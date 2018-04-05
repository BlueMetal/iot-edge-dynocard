module powerbi.extensibility.visual {
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