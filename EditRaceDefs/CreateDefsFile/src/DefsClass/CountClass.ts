import { ApparelItem } from "./Apparel"

export const ApparelLayerList = {
    AS_legs: "具足類",
    AS_UnderMiddle: "肌着類",
    AS_Neck: "マフラー類",
    AS_Coat: "コート類",
    AS_Backpack: "バックパック類"
} as const;

export class ItemListClass {
    categoryName: string = "";
    items: ItemsClass[] = new Array<ItemsClass>();
    itemCount: number = 0;
    constructor(name: string) {
        this.categoryName = name;
    }
}

export interface ItemsClass {
    itemName: string;
}

export class LayerLsitByItem {
    itemName: string;
    layers: ApparelItem.DefaultOutfitTags[];
    constructor(name: string, layers: ApparelItem.DefaultOutfitTags[]) {
        this.itemName = name;
        this.layers = layers;
    }
}