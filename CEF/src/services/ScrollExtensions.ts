import {Directive} from "vue";

export const vHorizontalScroll: Directive = {
  mounted: (el: HTMLElement) => {
    const stepWidth = (el.children[0] as HTMLElement).offsetWidth;
    el.addEventListener("wheel", (event) => {
      event.preventDefault();
      if(event.deltaY > 0) el.scrollLeft += stepWidth;
      else el.scrollLeft -= stepWidth;
    });
  }
}