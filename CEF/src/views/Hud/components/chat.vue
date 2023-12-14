<template>
  <div :class="$style.chat">
    <div :class="$style.messages" v-scroll-bottom="messages.length">
      <div :class="$style.message" v-for="(message, i) in messages" :key="i">
        <div :class="$style.header">
        <span :class="$style.type" v-if="message.newsPhone !== undefined">
          <TypeNewsIcon :class="$style.typeIcon"/>
        </span>
          <span v-html="message.header"/>
          <template v-if="message.newsPhone !== undefined">
            <span :class="$style.iconAction"><MessageIcon/></span>
            <span :class="$style.iconAction"><CallIcon/></span>
          </template>
        </div>
        <div v-if="message.text" :class="$style.text" v-html="message.text"/>
      </div>
    </div>
    <div :class="$style.inputContainer" v-show="isShow" v-click-out-element>
      <div :class="$style.select" @click="isSelectShow = !isSelectShow">
        <span>{{ currAlias.name }}</span>
        <SwitchIcon :class="{[$style.open]: isSelectShow}"/>
        <div :class="$style.selectCmd" :style="{ maxHeight: isSelectShow ? '40vh' : '0' }">
          <div :class="$style.cmd" v-for="(Command, Index) in CommandAliases.filter(x => x !== currAlias)" :key="Index"
               @click="playerSelect(Command)">
            <span>{{ Command.name }}</span>
            <span style="opacity: 0.5; font-size: 1.1vh;" v-if="Command.placeholder">{{ Command.placeholder }}</span>
          </div>
        </div>
      </div>
      <div :class="$style.inputValue">
        <span :class="$style.inputAlias" v-if="alias != null">{{ alias }}</span>
        <input :class="[$style.input, alias != null ? $style.alias : '']" placeholder="Напишите текст..."
               v-model="input"
               @keyup.enter="playerSendMessage" @keyup.esc="isShow = false" v-focus="isShow"
        @keyup.up="messageSwitch++" @keyup.down="messageSwitch--">
      </div>
      <div :class="{[$style.send]: true, [$style.nosend]: input.length === 0}" @click="playerSendMessage">
        <SendIcon/>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import TypeNewsIcon from "@images/chat/type-news.svg";
import CallIcon from "@images/chat/call-icon.svg";
import MessageIcon from "@images/chat/message-icon.svg";
import SwitchIcon from "@images/chat/switch-icon.svg";
import SendIcon from "@images/chat/send-icon.svg";
import {Directive, onMounted, onUnmounted, reactive, watch} from "vue";
import {Mp} from "../../../services/Alt";
import {Key, KeyCode} from "../../../services/KeyManager";
import {vFocus} from "../../../directives/Focus";
import {vScrollBottom} from "../../../directives/ScrollBottom";

interface IChatMessage {
  header: string;
  text?: string;
  newsPhone?: number;
}

const vClickOutElement: Directive = {
  mounted(el) {
    window.addEventListener("mouseup", (event) => {
      if(!el.contains(event.target)) isShow = false;
    });
  }
}
const messageHistory = reactive<string[]>([]);
let messageSwitch = $ref(-1);
let messageTemp: string | undefined;
watch(() => messageSwitch, (_, oldVal) => {
  console.log(messageSwitch);
  if(oldVal === -1) messageTemp = input;
  if(messageSwitch === -2) messageSwitch = messageHistory.length - 1;
  else if(messageSwitch >= messageHistory.length) messageSwitch = -1;
  if(messageSwitch !== -1) input = messageHistory[messageSwitch];
  else input = messageTemp!;
  console.log(messageSwitch);
});
const messages = reactive<IChatMessage[]>([]);
let input = $ref('');
let isShow = $ref(false);

interface ICommandAliasInfo {
  name: string;
  placeholder?: string;
  command?: string;
}

const CommandAliases = reactive<ICommandAliasInfo[]>([
  {
    name: "Ролевой"
  },
  {
    name: "Не ролевой",
    command: "b"
  },
  {
    name: "Организация",
    command: "r"
  },
  {
    name: "Организация",
    placeholder: "Не ролевой",
    command: "rb"
  },
  {
    name: "Департамент",
    command: "d"
  },
  {
    name: "Семья",
    command: "f"
  },
  {
    name: "Действие",
    placeholder: "От 1-го лица",
    command: "me"
  },
  {
    name: "Действие",
    placeholder: "От 3-го лица",
    command: "do"
  },
  {
    name: "Действие",
    placeholder: "Успех/провал",
    command: "try"
  }
]);

let isSelectShow = $ref(false);
let alias = $ref<string | null>(null);
let currAlias = $ref<ICommandAliasInfo>(CommandAliases[0]);

let key: Key | undefined;
watch(() => input, () => {
  if (input.length > 0 && input[0] == '/' && input.slice(-1) == ' ') {
    const args = input.split(' ');
    const index = CommandAliases.find(x => x.command == args[0].replace('/', '').toLowerCase());
    if (!index) return;
    alias = args[0];
    currAlias = index;
    input = input.replace(args[0] + ' ', '');
  }
});

watch(() => alias, () => {
  if (!alias) currAlias = CommandAliases[0];
  else if (key === undefined) {
    key = new Key(KeyCode.Backspace, () => {
      if (input.length != 0) return;
      alias = null;
    }, {inputIgnore: true, down: true});
  } else if (alias) {
    key.remove();
    key = undefined;
  }
})

function playerSelect(command: ICommandAliasInfo) {
  if (!command.command) alias = null;
  else {
    currAlias = command;
    alias = '/' + command.command;
  }
}

function playerSendMessage() {
  isShow = false;
  if (input.length === 0) return;
  Mp.emit("Chat:Input", alias !== null && input[0] !== "/" ? alias + ' ' + input : input);
  messageHistory.unshift(input);
  input = "";
}

watch(() => isShow, () => {
  Mp.emit("CursorState", isShow);
  isSelectShow = false;
});

Mp.events.temp("Chat:Send", (header: string, text?: string, phone?: number) => {
  messages.push({header, text, newsPhone: phone});
});
Key.temp(KeyCode.T, () => isShow = true);
</script>

<style lang="stylus" module>
.chat
  pos(top:3vh)
  width 41.5vh
  height 29vh
  font-weight 400
  font-family DinDisplay

.messages
  width 100%
  height 87%
  padding-left 0.3vw
  flex(column)
  overflow-y auto
  overflow-x hidden

  &::-webkit-scrollbar
    appearance none
  padding-bottom 1vh

.message
  text-shadow: 0 0.05vh 0.2vh rgba(0, 0, 0, 0.6);
  margin-top 0.7vh

.header
  flex(row)

.nick
  color #fff

.type
  margin-right 0.3vw
  flex(cont:center, ialign:center)
  height 2vh
  width 2vh
  background radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, #494870 15.34%, #49486F 36.08%, #525175 68.89%, #595884 84.96%);
  border 0.1vh solid
  border-image-source linear-gradient(261.13deg, rgba(46, 45, 75, 0.3) 23.54%, rgba(46, 45, 75, 0.09) 61.33%)
  border-image-slice 1
  border-radius 0.3vh

.typeIcon
  height 75%

.iconAction
  margin-left 0.2vw
  width 2vh
  height 2vh
  background radial-gradient(50% 49.41% at 50% 50.59%, rgba(250, 250, 250, 0.6) 0%, rgba(255, 255, 255, 0.6) 14.38%, rgba(241, 241, 241, 0.6) 50%, rgba(222, 222, 222, 0.6) 100%)
  border 0.1vh solid #F8F8F899
  opacity 0.75
  border-radius: 0.2vh;

  & svg
    width 100%

  &:hover
    opacity 1

.inputContainer
  width 100%
  height 12%
  background rgba(23, 23, 23, 0.9)
  flex(ialign:center)

.select
  position relative
  font-family DinDisplay
  font-weight 500
  color #242424
  font-size 1.6vh
  flex(cont:space-evenly, ialign:center)
  width 22.5%
  height 100%
  background #fff

  & svg
    position absolute
    bottom 0.2vh
    width 0.5vw
    transition all 0.1s ease

    &.open
      transform rotate(180deg)

.inputValue
  width 68.5%
  flex(row, ialign:center)

.inputAlias
  margin-left 0.3vw
  color rgba(255, 255, 255, 0.4)

.input
  width 100%
  font-size 1.4vh
  padding 0 0.9vw
  font-family DinDisplay
  text-shadow: 0 0.1vh 0.1vh rgba(0, 0, 0, 0.15)
  appearance none
  color #fff

  &.alias
    padding 0 0.2vw

  &::-webkit-input-placeholder
    color #fff

.send
  flex(cont:center, ialign:center)
  width 9%
  height 100%

  &.nosend

    & path
      fill-opacity 0.15

  & svg
    height 50%

.selectCmd
  position absolute
  top 100%
  width 100%
  font-size 1.3vh
  font-weight 300
  background rgba(23, 23, 23, 0.9)
  overflow hidden
  max-height 0
  transition all 0.2s ease-in

.cmd
  color #fff
  flex(column, center)
  height 3.8vh
  padding-left 0.3vw
  line-height 1.5vh
  letter-spacing 0.02vw
  transition all 0.1s ease

  &:not(:last-child)
    border-bottom 0.1vh solid #D3D3D3

  &:hover
    background rgba(211, 211, 211, 0.4)
    border-bottom 0.1vh solid transparent
</style>