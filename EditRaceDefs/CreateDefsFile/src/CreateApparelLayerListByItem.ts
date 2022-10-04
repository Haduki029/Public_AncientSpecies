import parser from "fast-xml-parser"
import fs from 'fs'
import {ApparelLayer} from "./DefsClass/ApparelLayer"
import {LayerLsitByItem} from "./DefsClass/CountClass"
import { ApparelItem } from "./DefsClass/Apparel"

/**
 * Apparel用。
 * ApparelアイテムごとのLayerリストを作成し出力する。
 */

async function main() {
    const options: parser.X2jOptionsOptional = {
      ignoreAttributes : false,
      ignoreDeclaration: true
    };
    // console.log(__dirname);
    const filePathList: string[] = fs.readdirSync(__dirname + "/targetFiles");

    const xp = new parser.XMLParser(options);
    let dataList = new Array<LayerLsitByItem>();

    for (let filePath of filePathList) {
      // console.log(filePath);
      // console.log(__dirname + "/targetFiles/" + filePath);
      const xmlContent = await fs.readFileSync(__dirname + "/targetFiles/" + filePath, 'utf-8')

      let data: ApparelItem.Root;
      const jObj = xp.parse(xmlContent);

      // const data: RaceDef.Root = jObj;
      data = jObj;
      // console.log(data);


      //// let targetData: string[] = new Array<string>();
      // 配列になっていないデータを配列化
      data.Defs.ThingDef = convertArrayItem(data.Defs.ThingDef);
      console.log(isArray(data.Defs.ThingDef));
      // console.log(isArray(data.Defs.ThingDef.apparel.layers.li));
      for (let thingDefData of data.Defs.ThingDef) {
        // 配列になっていないデータを配列化
        thingDefData.apparel.layers = convertArrayItem(thingDefData.apparel.layers);
        let dataListItem: LayerLsitByItem = new LayerLsitByItem(thingDefData.defName, thingDefData.apparel.layers);

        dataList.push(dataListItem);
      }
    }

    // console.log(dataList);
    writeData(dataList);

    // const builder = new parser.XMLBuilder(options);
    // const xmlContentOutput = builder.build(data);
    // console.log(xmlContentOutput);
}

function isArray(item: any) {
  const isArray = <T>(maybeArray: T | readonly T[]): maybeArray is T[] => {
    return Array.isArray(maybeArray);
  };
  return isArray(item);
}

function convertArrayItem(item: any) {
  if (isArray(item)) {
    return item;
  } else {
    let arr: Array<any> = new Array();
    arr.push(item);
    return arr;
  }
}

function writeData(targetData: LayerLsitByItem[]): void {
  const outPutData = JSON.stringify(targetData, null, 2);
  fs.writeFileSync(__dirname + '/ApparelLayesByItem.json', outPutData);
}

(async () => {
    await main();
  })().catch((e) => {
    console.warn(e);
  });
