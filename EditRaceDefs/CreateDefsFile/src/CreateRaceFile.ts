import parser from "fast-xml-parser"
import fs from 'fs'
import {ApparelLayer} from "./DefsClass/ApparelLayer"
import {RaceDef} from "./DefsClass/ThingDef_Race"

async function main() {
    // console.log(__dirname);
    const xmlContent = await fs.readFileSync(__dirname + '/Race_WHE.xml', 'utf-8')

    const options: parser.X2jOptionsOptional = {
        ignoreAttributes : false,
        ignoreDeclaration: true
    };

    const xp = new parser.XMLParser(options);

    const jObj = xp.parse(xmlContent);
    // console.log(jObj);

    const data: RaceDef.Root = jObj;

    console.log(data);
    console.log(data.Defs);
    const outPutData = JSON.stringify(data, null, 2);
    fs.writeFileSync('output.json', outPutData);

    // const builder = new parser.XMLBuilder(options);
    // const xmlContentOutput = builder.build(data);
    // console.log(xmlContentOutput);
    
}

(async () => {
    await main();
  })().catch((e) => {
    console.warn(e);
  });
