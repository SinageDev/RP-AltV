import {Directive} from "vue";

export const vScrollBottom: Directive = {
  updated(el, binding) {
    if(binding && binding.oldValue == binding.value) return;
    el.scrollTo(0, el.scrollHeight);
  }
};