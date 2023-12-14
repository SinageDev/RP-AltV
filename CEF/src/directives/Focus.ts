import {Directive, nextTick} from "vue";

export const vFocus: Directive = {
  updated(el, binding) {
    if(binding.value) nextTick(() => el.focus());
  }
};