

module powerbi.extensibility.visual {
    "use strict";
    export class Visual implements IVisual {
        private target: HTMLElement;
        private containerDiv: HTMLElement;
        private settings: VisualSettings;
        private textNode: Text;
        private flagCounter: number;
        private isDropDownRender: boolean = false;

        private refoptions: VisualUpdateOptions;

        //-- declaration Block--------------
        private host: IVisualHost;

        private surCardSVG: d3.Selection<SVGAElement>;
        private surCrdSvgGrp: d3.Selection<SVGAElement>;
        private pumpCardSVG: d3.Selection<SVGAElement>;
        private pumpCrdSvgGrp: d3.Selection<SVGAElement>;



        private dataSet: ViewModel;
        // private pumpIdVal = null;
        private eventIdVal: any = 'all';
        private cardTypeVal: any = 'all';
        private eventIdDDList;
        private cardTypeDDList;
        private graphData: DataPoint[];
        private plotteSurfacedPath: any;
        private plottePumpPath: any;


        constructor(options: VisualConstructorOptions) {
            this.host = options.host;
            this.flagCounter = 0;
            this.target = options.element;

            if (typeof document !== "undefined") {
                this.target.appendChild(this.createInitialHeader());
                let animateButton = this.createButton();
                document.getElementById("buttonDiv").appendChild(animateButton);
                this.surCardSVG = d3.select(document.getElementById("surfaceCard")).append("svg").classed("sur-svg-cls", true);
                this.surCrdSvgGrp = this.surCardSVG.append("g").classed("sur-svg-grp-cls", true);
                this.pumpCardSVG = d3.select(document.getElementById("pumpCardDiv")).append("svg").classed("pump-svg-cls", true);
                this.pumpCrdSvgGrp = this.pumpCardSVG.append("g").classed("pump-svg-grp-cls", true);

            }
        }

        public update(options: VisualUpdateOptions) {
            this.dataSet = this.getTableData(options);
            if (!this.isDropDownRender) {
                console.log("Going to Render update Points");
                let pumpDD = this.createDropDown(DataColumns.pumpId);
                let eventDD = this.createDropDown(DataColumns.eventId);
                let cardTypeDD = this.createDropDown(DataColumns.cardType);
                document.getElementById("controlDiv").appendChild(pumpDD);
                document.getElementById("controlDiv").appendChild(eventDD);
                document.getElementById("controlDiv").appendChild(cardTypeDD);
                this.isDropDownRender = true;
            }
            this.updatePoints(options);
            this.refoptions = options;
        }

        private updatePoints(options: VisualUpdateOptions) {

            let svgWidthFull = options.viewport.width;
            let svgHeightFull = options.viewport.height / 2;
            let margin = { top: 100, right: 5, bottom: 10, left: 5 }
            let svgWidth = svgWidthFull - margin.left - margin.right;
            let svgHeight = svgHeightFull - margin.top - margin.bottom;
            this.surCardSVG.attr({
                width: svgWidth,
                height: svgHeight
            });
            this.pumpCardSVG.attr({
                width: svgWidth,
                height: svgHeight
            });




            // this.barGroup.attr("transform", "translate(0,5)");

            let surfCardData = _.filter(this.graphData, { 'cardType': 'S' });
            let pumpCardData = _.filter(this.graphData, { 'cardType': 'P' });

            console.log("Surface Data Before Short", surfCardData);
            surfCardData = _.sortBy(surfCardData, 'cardId');
            //surfCardData= _.sortBy(surfCardData, 'load');
            console.log("Surface Data After Short", surfCardData);
            let xMaxPos_surf = d3.max(surfCardData, d => d.position);
            let yMaxLoad_surf = d3.max(surfCardData, d => d.load);
            let xAxisPos_surf = d3.scale.linear().domain([0, xMaxPos_surf]).range([0, svgWidth]);
            let yAxisLoad_surf = d3.scale.linear().domain(d3.extent(surfCardData, d => d.load)).range([svgHeight, 0])

            let xMaxPos_pump = d3.max(pumpCardData, d => d.position);
            let yMaxLoad_pump = d3.max(pumpCardData, d => d.load);
            let xAxisPos_pump = d3.scale.linear().domain([0, xMaxPos_surf]).range([0, svgWidth]);
            let yAxisLoad_pump = d3.scale.linear().domain(d3.extent(pumpCardData, d => d.load)).range([svgHeight, 0])


            const drawLine: d3.svg.Line<DataPoint> = d3.svg.line<DataPoint>().interpolate("cardinal")
                .x((dp: DataPoint) => { return xAxisPos_surf(dp.position); })
                .y((dp: DataPoint) => { return yAxisLoad_surf(dp.load); });


            const drawPumpLine: d3.svg.Line<DataPoint> = d3.svg.line<DataPoint>().interpolate("cardinal")
                .x((dp: DataPoint) => { return xAxisPos_pump(dp.position); })
                .y((dp: DataPoint) => { return yAxisLoad_pump(dp.load); });
                
            let plotSurfacePath = this.surCrdSvgGrp.selectAll("path").data([surfCardData]);
            plotSurfacePath.enter().append("path").classed("path-cls", true);
            plotSurfacePath.exit().remove();
            plotSurfacePath.attr("stroke", "red")
                .attr("stroke-width", 2)
                .attr("fill", "none")
                .attr("d", drawLine);

            this.plotteSurfacedPath = d3.select(document.getElementById("surfaceCard")).selectAll("path");
            let surfacePathLength = this.plotteSurfacedPath.node().getTotalLength();
            console.log("plotPathLenght Lenght", surfacePathLength);

            plotSurfacePath
                .attr("stroke-dasharray", surfacePathLength + " " + surfacePathLength)
                .attr("stroke-dashoffset", surfacePathLength)
                .transition()
                .duration(2000)
                .ease("linear")
                .attr("stroke-dashoffset", 0);



            let plotPumpPath = this.pumpCrdSvgGrp.selectAll("path").data([pumpCardData]);
            plotPumpPath.enter().append("path").classed("path-cls", true);
            plotPumpPath.exit().remove();
            plotPumpPath.attr("stroke", "steelblue")
                .attr("stroke-width", 2)
                .attr("fill", "none")
                .attr("d", drawPumpLine);


            this.plottePumpPath = d3.select(document.getElementById("pumpCardDiv")).selectAll("path");
            let pumpPathLength = this.plottePumpPath.node().getTotalLength();
                console.log("pumpPathLength Lenght", pumpPathLength);
    
                plotPumpPath
                    .attr("stroke-dasharray", pumpPathLength + " " + pumpPathLength)
                    .attr("stroke-dashoffset", pumpPathLength)
                    .transition()
                    .duration(2000)
                    .ease("linear")
                    .attr("stroke-dashoffset", 0);
        

            // let dotPump = this.pumpCrdSvgGrp.selectAll("circle").data(pumpCardData);
            // dotPump.enter().append("circle").attr({
            //     r: 2,
            //     cy: d => yAxisLoad_pump(d.load),
            //     cx: d => xAxisPos_pump(d.position)
            // }).style({
            //     fill: 'blue'
            // });
            // dotPump.exit().remove();
            // dotPump.attr({
            //     r: 2,
            //     cy: d => yAxisLoad_pump(d.load),
            //     cx: d => xAxisPos_pump(d.position)
            // }).style({
            //     fill: 'blue'
            // })




        }
        private static parseSettings(dataView: DataView): VisualSettings {
            return VisualSettings.parse(dataView) as VisualSettings;
        }

        /** 
         * This function gets called for each of the objects defined in the capabilities files and allows you to select which of the 
         * objects and properties you want to expose to the users in the property pane.
         */
        public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstance[] | VisualObjectInstanceEnumerationObject {
            return VisualSettings.enumerateObjectInstances(this.settings || VisualSettings.getDefault(), options);
        }


        public getTableData(options: VisualUpdateOptions): ViewModel {
            console.log("Options: ", options);
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
                    cardHeaderId: <number>dataView[i][columnPos.indexOf(DataColumns.cardHeaderId)],
                    cardType: <string>dataView[i][columnPos.indexOf(DataColumns.cardType)],
                    cardId: <number>dataView[i][columnPos.indexOf(DataColumns.cardId)],
                    position: <number>dataView[i][columnPos.indexOf(DataColumns.position)],
                    load: <number>+dataView[i][columnPos.indexOf(DataColumns.load)]
                });
            }
            console.log("Return Data:", retDataView);
            return retDataView;
        }



        public createInitialHeader() {
            const baseDiv: HTMLElement = document.createElement("div");
            baseDiv.setAttribute("class", "container-fluid");

            const reportTitle: HTMLElement = document.createElement("p");
            reportTitle.setAttribute("class", "text-center")
            reportTitle.appendChild(document.createTextNode(" Graph: Dyno Card"));
            baseDiv.appendChild(reportTitle);

            const controlDiv: HTMLElement = document.createElement("div");
            controlDiv.setAttribute("id", "controlDiv");
            controlDiv.setAttribute("class", "form-inline");
            baseDiv.appendChild(controlDiv);

            const surfaceCardDiv: HTMLElement = document.createElement("div");
            surfaceCardDiv.setAttribute("id", "surfaceCard");
            surfaceCardDiv.setAttribute("class", "row");
            baseDiv.appendChild(surfaceCardDiv);

            baseDiv.appendChild(document.createElement("hr"));

            const pumpCardDiv: HTMLElement = document.createElement("div");
            pumpCardDiv.setAttribute("id", "pumpCardDiv");
            pumpCardDiv.setAttribute("class", "row");
            baseDiv.appendChild(pumpCardDiv);

            baseDiv.appendChild(document.createElement("hr"));
            const buttonDiv: HTMLElement = document.createElement("div");
            buttonDiv.setAttribute("id", "buttonDiv");
            buttonDiv.setAttribute("class", "row");
            baseDiv.appendChild(buttonDiv);

            return baseDiv;
        }

        public createDropDown(argDropDownType: string) {
            let ddDiv = document.createElement("div");
            let ddLabel: HTMLElement;
            ddDiv.setAttribute("class", "col-xs-4 input-group");
            let labelDiv = document.createElement("div");
            labelDiv.setAttribute("class", "input-group-addon");
            let dropDown = document.createElement("select");
            dropDown.setAttribute("class", "form-control");
            dropDown.setAttribute("id", argDropDownType);
            let dropDownData = [];

            if (argDropDownType == DataColumns.pumpId) {
                labelDiv.appendChild(document.createTextNode("Pump ID"))
                let pumpIdList = _.uniq(_.map(this.dataSet.dataPoints, 'pumpId'));
                dropDownData = _.map(pumpIdList, item => String(item))
            } else if (argDropDownType == DataColumns.cardType) {
                labelDiv.appendChild(document.createTextNode("Card Type"))
                dropDownData = _.uniq(_.map(this.dataSet.dataPoints, 'cardType'));
                this.cardTypeDDList = dropDownData;
            } else if (argDropDownType == DataColumns.eventId) {
                labelDiv.appendChild(document.createTextNode("Event ID"))
                dropDownData = _.uniq(_.map(this.dataSet.dataPoints, 'eventId'));
                this.eventIdDDList = dropDownData;
            }

            //Create and append the options
            let allOp = document.createElement("option");
            allOp.text = "All";
            allOp.value = "all";
            dropDown.appendChild(allOp);
            for (let i = 0; i < dropDownData.length; i++) {
                let option = document.createElement("option");
                option.value = dropDownData[i];
                option.text = dropDownData[i];
                dropDown.appendChild(option);
            }
            dropDown.onchange = (event: Event) => {
                let selVal = $("#" + argDropDownType).val();
                console.log("Event for: ", argDropDownType, ' value: ', selVal);
                if (argDropDownType == DataColumns.eventId) this.eventIdVal = selVal;
                else if (argDropDownType == DataColumns.cardType) this.cardTypeVal = selVal;
                this.updateGraphData();
                this.updatePoints(this.refoptions);
                console.log("Current Graph Data:", this.graphData);
            }

            ddDiv.appendChild(labelDiv);
            ddDiv.appendChild(dropDown);
            return ddDiv;
        }

        public updateGraphData() {
            if (this.eventIdVal == 'all' && this.cardTypeVal == 'all') this.graphData = this.dataSet.dataPoints;
            else if (this.eventIdVal != 'all' && this.cardTypeVal == 'all') this.graphData = _.filter(this.dataSet.dataPoints, { 'eventId': +this.eventIdVal });
            else if (this.eventIdVal == 'all' && this.cardTypeVal != 'all') this.graphData = _.filter(this.dataSet.dataPoints, { 'cardType': this.cardTypeVal });
            else if (this.eventIdVal != 'all' && this.cardTypeVal != 'all') {
                let graphDataFilterByEventId = _.filter(this.dataSet.dataPoints, { 'eventId': +this.eventIdVal });
                this.graphData = _.filter(graphDataFilterByEventId, { 'cardType': this.cardTypeVal });
            }

        }

        public createButton() {
            let tempButton = document.createElement("button");
            let clickCount = 0;
            let thisRef = this;
            tempButton.setAttribute("type", "button");
            tempButton.setAttribute("class", "btn btn-success center-block");
            tempButton.textContent = "Run DynoCard Animation";
            tempButton.onclick = function () {
                thisRef.flagCounter++;
                thisRef.updatePoints(thisRef.refoptions);
            }
            return tempButton;
        }

    }

    export class DataColumns {
        static pumpId = "PumpId";
        static eventId = "EventId";
        static cardHeaderId = "CardHeaderID";
        static cardType = "CardType";
        static cardId = "CardId";
        static position = "Postition";
        static load = "Load";
    }
    export interface DataPoint {
        pumpId: number;
        eventId: number;
        cardHeaderId: number;
        cardType: string;
        cardId: number;
        position: number;
        load: number;
    };

    export interface ViewModel {
        dataPoints: DataPoint[];
        maxValue: number;
    }
}