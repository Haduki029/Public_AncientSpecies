import parser from "fast-xml-parser"
import fs from 'fs'
import {ApparelLayer} from "./DefsClass/ApparelLayer"
import {RaceDef} from "./DefsClass/ThingDef_Race"
import {ApparelLayerList, ItemListClass, ItemsClass} from "./DefsClass/CountClass"

/**
 * Apparel用。
 * categoryName別に前アイテムと個数を出力。
 */

async function main() {
    const options: parser.X2jOptionsOptional = {
      ignoreAttributes : false,
      ignoreDeclaration: true
    };
    // console.log(__dirname);
    const filePathList: string[] = fs.readdirSync(__dirname + "/targetFiles");

    const xp = new parser.XMLParser(options);
    let dataList = new Array<ItemListClass>();

    for (let filePath of filePathList) {
      // console.log(filePath);
      // console.log(__dirname + "/targetFiles/" + filePath);
      const xmlContent = await fs.readFileSync(__dirname + "/targetFiles/" + filePath, 'utf-8')

      const jObj = xp.parse(xmlContent);
      // const data: RaceDef.Root = jObj;
      const data: any = jObj;
      // console.log(data);

      const isIterable = (value: any) => {
        return Symbol.iterator in Object(value);
      }

      //// let targetData: string[] = new Array<string>();
      if (!isIterable(data.Defs.ThingDef)) {
        // iterableじゃない場合の処理
        let flg: boolean = false;
          flg = dataList.some((itemData) => itemData.categoryName === data.Defs.ThingDef.apparel.layers.li);
          if (flg) {
            dataList.forEach(itemData => {
              if (itemData.categoryName === data.Defs.ThingDef.apparel.layers.li) {
                itemData.items.push(data.Defs.ThingDef.defName);
                ++itemData.itemCount;
              }
            });
          } else {
            let item = new ItemListClass(data.Defs.ThingDef.apparel.layers.li);
            item.items.push(data.Defs.ThingDef.defName);
            ++item.itemCount;
            dataList.push(item);
          }
      } else {
        for (let thingDefData of data.Defs.ThingDef) {
          let flg: boolean = false;
          flg = dataList.some((data) => data.categoryName === thingDefData.apparel.layers.li);
          if (flg) {
            dataList.forEach(data => {
              if (data.categoryName === thingDefData.apparel.layers.li) {
                data.items.push(thingDefData.defName);
                ++data.itemCount;
              }
            });
          } else {
            // let defName = selectCategoryName(thingDefData.apparel.layers.li);
            // let item = new ItemListClass(defName);
            let item = new ItemListClass(thingDefData.apparel.layers.li);
            item.items.push(thingDefData.defName);
            ++item.itemCount;
            dataList.push(item);
          }
        }
      }
    }

    // console.log(dataList);
    writeData(dataList);

    // const builder = new parser.XMLBuilder(options);
    // const xmlContentOutput = builder.build(data);
    // console.log(xmlContentOutput);
}

function selectCategoryName(layer: string): string {
  switch(layer) {
    case "AS_legs":
      return "具足類";
    case "AS_UnderMiddle":
      return "肌着類";
    case "AS_Neck":
      return "マフラー類";
    case "AS_Coat":
      return "コート類";
    case "AS_Backpack":
      return "バックパック類";
    default:
      return layer;
  }
}

function writeData(targetData: ItemListClass[]): void {
  const outPutData = JSON.stringify(targetData, null, 2);
  fs.writeFileSync(__dirname + '/output.json', outPutData);
}

(async () => {
    await main();
  })().catch((e) => {
    console.warn(e);
  });
