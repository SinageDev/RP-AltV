interface ICharacterInfo {
  Name: string;
  Money: number;
  Level: number;
  Exp: number;
  RegDate: Date;
  LastDate?: string;
  Warn: number;
  Fraction?: number[];
  Business?: number[];
  Homes?: number[];
  Gender: boolean;
  Customization: IPedCustomization;
}

interface IPedCustomization {
  HeadBlendData: IHeadBlendData;
  FaceFeatures: Map<number, number>;
  HeadOverlays: Map<number, IHeadOverlay>;
  Hair: [number, number, number];
  EyeColor: number;
}

interface IHeadBlendData {
  Mather: number;
  Father: number;
  Shape: number;
  Skin: number;
}

interface IHeadOverlay {
  Index: number;
  FirstColor: number;
  SecondColor: number;
  Opacity: number;
}

interface IFaceFeatures {
  [key: string]: number;
}

declare module "arp-inventory" {
  interface IItems {
    [key: number]: InventoryItem;
  }

  export class Inventory {
    public Name: string;
    public Slots: InventorySlot[];
    public InventoryId: number;
  }

  export class CharacterInventory
  {
    public UsedSlots: UseSlots;
    public MainSlots: Inventory;
    public Backpack?: Inventory;
    public AnotherSlots?: Inventory;
  }

  export class InventorySlot
  {
    public ItemId?: number;
    public KeyBind?: number;
    public Count?: number;
    public Durability?: number;
  }

  export class UseSlots
  {
    public Clothes: InventorySlot[];
    public Backpack?: InventorySlot;
  }

  export abstract class InventoryItem {
    public Name: string;
    public Description: string;
  }

  export class ItemBackpack extends InventoryItem {
    public Slots: number;
  }
}