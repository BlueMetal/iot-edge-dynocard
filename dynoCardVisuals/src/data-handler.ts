module powerbi.extensibility.visual {
    export class DataHandler{
        public static getTableData(options: VisualUpdateOptions): ViewModel {
            let sampleData: DataPoint[] = [];
            let retDataView: ViewModel = {
                dataPoints: sampleData,
                maxValue: d3.max(sampleData, d => d.load)
            }

            let dataView = options.dataViews[0].table.rows;
            let columnArr = options.dataViews[0].table.columns;
            let columnPos: any[] = [];
            for (let i = 0; i < columnArr.length; i++) {
                columnPos.push(String(Object.keys(columnArr[i].roles)[0]));
            }
            for (let i = 0; i < dataView.length; i++) {
                retDataView.dataPoints.push({
                    pumpId: <number>+dataView[i][columnPos.indexOf(DataColumns.pumpId)],
                    eventId: <number>+dataView[i][columnPos.indexOf(DataColumns.eventId)],
                    cardHeaderId: <number> +dataView[i][columnPos.indexOf(DataColumns.cardHeaderId)],
                    epocDate: <number>+dataView[i][columnPos.indexOf(DataColumns.epocDate)],
                    cardType: <string>dataView[i][columnPos.indexOf(DataColumns.cardType)],
                    cardId: <number>dataView[i][columnPos.indexOf(DataColumns.cardId)],
                    position: <number>dataView[i][columnPos.indexOf(DataColumns.position)],
                    load: <number>+dataView[i][columnPos.indexOf(DataColumns.load)]
                });
            }

            return retDataView;
        }
    }
}