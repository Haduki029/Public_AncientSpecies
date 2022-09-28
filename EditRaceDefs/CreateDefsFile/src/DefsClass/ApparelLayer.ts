export namespace ApparelLayer {
    export interface Root {
        Defs: Defs;
    }
    
    export class Defs {
        ApparelLayerDef: ApparelLayerDef[] = new Array<ApparelLayerDef>;
    }
    
    export class ApparelLayerDef {
        defName:   string = "";
        label:     string = "";
        drawOrder: string = "";
    }
}

