module powerbi.extensibility.visual.dynoCardVisuals8DD0D1F7BB764FE1A1556C3E004ED3E3  {
    export interface DataPoint {
        pumpId: number;
        eventId: number;
        cardHeaderId: number;
        epocDate:Date;
        cardType: string;
        cardId: number;
        position: number;
        load: number;
    };
}