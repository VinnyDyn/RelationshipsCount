import { RelationshipsCount } from "..";
import { Resx } from "./Resx";

let component: RelationshipsCount;
let languageFile: Resx;
export class RelationshipsCountData {
    public static AddContext(caller: RelationshipsCount, resx: Resx) {
        component = caller;
        languageFile = resx;
    }

    public static Refresh() {
        component.Refresh();
    }

    public static Resx(): Resx {
        return languageFile;
    }
}