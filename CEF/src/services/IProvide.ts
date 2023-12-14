import {Ref} from "vue";

export interface IProvideCreator {
  parent: number[];
  gender: Ref<boolean>;
  hair: number[];
  overlay: number[][];
  eye: Ref<number>;
  name: Ref<string>;
  nameError: Ref<string>;
  family: Ref<string>;
  familyError: Ref<string>;
  step: Ref<number>;
  maleHairIds: number[];
  feMaleHairIds: number[];
  OverlayInfo: {
    header: string;
    selects: number[];
    color?: string;
  }[];
  eyeIds: number[];
  targetOverlay: Ref<number | null>;
}

export interface IProvideSelect {
  max: Ref<number>;
  characters: Ref<ICharacterInfo[]>;
  current: Ref<number>;
}