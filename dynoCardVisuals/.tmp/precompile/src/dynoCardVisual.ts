
module powerbi.extensibility.visual.dynoCardVisuals8DD0D1F7BB764FE1A1556C3E004ED3E3  {

    export class DynoCardVisual implements IVisual {

        //---Power Bi
        private target: HTMLElement;
        private host: IVisualHost;
        private settings: VisualSettings;
        private refoptions: VisualUpdateOptions;

        //--- SVG 
        private dynoCardSvg: d3.Selection<SVGAElement>;
        private surCrdSvgGrp: d3.Selection<SVGAElement>;
        private pumpCrdSvgGrp: d3.Selection<SVGAElement>;
        private drawLineFunc: d3.svg.Line<DataPoint>;
        private svgCanvasHeight: number;

        // axis
        private xAxisGroup: d3.Selection<SVGAElement>;
        private yAxisGroupSurface: d3.Selection<SVGAElement>;
        private yAxisGroupPump: d3.Selection<SVGAElement>;
        private xAxis_Position;
        private yAxis_Load;

        private dataSet: ViewModel;
        private eventSelVal: any = 'all';
        private pumpSelVal: any = 'all';
        private eventIdDDList;
        private cardTypeDDList;
        private plotteSurfacedPath: any;
        private plottePumpPath: any;
        private isDropDownRender: boolean = false;
        private margin = { top: 100, right: 50, bottom: 80, left: 5 }
        private totalAnimationTime :number = 2000;

        constructor(options: VisualConstructorOptions) {
            this.host = options.host;
            this.target = options.element;

            if (typeof document !== "undefined") {
                this.target.appendChild(HtmlControl.createInitialHeader());
                let animateButton = HtmlControl.createAnimationButton(this);
                document.getElementById("buttonDiv").appendChild(animateButton);

                this.dynoCardSvg = d3.select(document.getElementById("dynoCardDiv")).append("svg").classed("dyno-svg-cls", true);

                this.surCrdSvgGrp = this.dynoCardSvg.append("g").classed("sur-svg-grp-cls", true);
                this.surCrdSvgGrp.attr({ id: "surfaceCard" });
                this.pumpCrdSvgGrp = this.dynoCardSvg.append("g").classed("pump-svg-grp-cls", true);
                this.pumpCrdSvgGrp.attr({ id: "pumpCard" });

                this.xAxisGroup = this.dynoCardSvg.append("g").classed("x-axis", true);
                this.yAxisGroupSurface = this.dynoCardSvg.append("g").classed("y-axis", true);
                this.yAxisGroupPump = this.dynoCardSvg.append("g").classed("y-axis-pump", true);
            }
        }

        public update(options: VisualUpdateOptions) {
            this.dataSet = DataHandler.getTableData(options);
            let svgCanvasWidth = options.viewport.width;
            this.svgCanvasHeight = options.viewport.height - this.margin.top - this.margin.bottom;
            this.dynoCardSvg.attr({
                width: svgCanvasWidth,
                height: this.svgCanvasHeight
            });

            var childNodes = document.getElementById("controlDiv");
            while (childNodes.firstChild) {
                childNodes.removeChild(childNodes.firstChild);
            }

            let pumpDD = this.createDropDown(DataColumns.pumpId);
            let eventDD = this.createDropDown(DataColumns.eventId);
            let stratDatePicker = HtmlControl.createDateTimePicker(DataColumns.startDate);
            let endDatePicker = HtmlControl.createDateTimePicker(DataColumns.endDate);
            document.getElementById("controlDiv").appendChild(pumpDD);
            document.getElementById("controlDiv").appendChild(stratDatePicker);
            document.getElementById("controlDiv").appendChild(endDatePicker);
            document.getElementById("controlDiv").appendChild(eventDD);

            this.dynoCardSvg.selectAll("line").remove();
            this.dynoCardSvg.append("line").attr({
                x1: this.margin.right,
                y1: this.svgCanvasHeight / 2,
                x2: svgCanvasWidth,
                y2: this.svgCanvasHeight / 2,
                "stroke-width": 0.5,
                "stroke": "gray"
            });

            //--- Define X & Y  Axis Scale and Line
            let xMax = d3.max(this.dataSet.dataPoints, d => d.position);
            this.xAxis_Position = d3.scale.linear().domain([-1, xMax]).range([0, svgCanvasWidth]);
            this.yAxis_Load = d3.scale.linear().domain(d3.extent(this.dataSet.dataPoints, d => d.load)).range([this.svgCanvasHeight / 2, 0]);

            let xAxisLine = d3.svg.axis().scale(this.xAxis_Position).orient("bottom").tickSize(5).tickFormat(d => d + ' in');
            this.xAxisGroup.call(xAxisLine).attr({
                transform: "translate(" + this.margin.right + ", " + (this.svgCanvasHeight - 20) + ")"
            });
            let yAxisLine = d3.svg.axis().scale(this.yAxis_Load).orient("left").tickSize(5).tickFormat(d => Number(d) / 1000 + ' klb');
            this.yAxisGroupSurface.call(yAxisLine).attr({
                transform: "translate(" + this.margin.right + ", 5)"
            });
            this.yAxisGroupPump.call(yAxisLine).attr({
                transform: "translate(" + this.margin.right + ", " + (this.svgCanvasHeight / 2 - 10) + ")"
            });

            //-- Define Path Draw function
            this.drawLineFunc = d3.svg.line<DataPoint>().interpolate("cardinal")
                .x((dp: DataPoint) => { return this.xAxis_Position(dp.position); })
                .y((dp: DataPoint) => { return this.yAxis_Load(dp.load); });

            //this.updateDynoCardGraph(options);
            this.animateGraph(this.updateGraphData());
            this.refoptions = options;
        }


        private renderCard(ci, surCardData, pumpCardData) {
            let plotSurfacePath = this.surCrdSvgGrp.selectAll("path" + ci).data([surCardData]);
            plotSurfacePath.enter().append("path").classed("path-cls", true);
            plotSurfacePath.exit().remove();
            plotSurfacePath.attr("stroke", "steelblue")
                .attr("stroke-width", 2)
                .attr("fill", "none")
                .attr("d", this.drawLineFunc);
            this.plotteSurfacedPath = d3.select(document.getElementById("surfaceCard")).selectAll("path");
            let surfacePathLength = this.plotteSurfacedPath.node().getTotalLength();

            plotSurfacePath
                .attr("stroke-dasharray", surfacePathLength + " " + surfacePathLength)
                .attr("stroke-dashoffset", surfacePathLength)
                .transition()
                .duration(1000)
                .ease("linear")
                .attr("stroke-dashoffset", 0);

            let plotPumpPath = this.pumpCrdSvgGrp.selectAll("path" + ci).data([pumpCardData]);
            plotPumpPath.enter().append("path").classed("path-cls", true);
            plotPumpPath.exit().remove();
            plotPumpPath.attr("stroke", "brown")
                .attr("stroke-width", 2)
                .attr("fill", "none")
                .attr("d", this.drawLineFunc);

            this.plottePumpPath = d3.select(document.getElementById("pumpCard")).selectAll("path");
            let pumpPathLength = this.plottePumpPath.node().getTotalLength();

            plotPumpPath
                .attr("stroke-dasharray", pumpPathLength + " " + pumpPathLength)
                .attr("stroke-dashoffset", pumpPathLength)
                .transition()
                .duration(1000)
                .ease("linear")
                .attr("stroke-dashoffset", 0);
            this.surCrdSvgGrp.attr({
                transform: "translate(10,0)"
            });
            this.surCrdSvgGrp.attr({
                transform: "translate(" + this.margin.right + ",0)"
            });
            this.pumpCrdSvgGrp.attr({
                transform: "translate(" + this.margin.right + "," + (this.svgCanvasHeight / 2 - 30) + ")"
            });
        }

        private animateGraph(argGraphDataSet: DataPoint[]) {
            let allDataPoints = _.sortBy(argGraphDataSet, 'cardId');
            let surfaceDataGrp = _.groupBy(_.filter(allDataPoints, { 'cardType': 'S' }), 'cardHeaderId');
            let pumpCardDataGrp = _.groupBy(_.filter(allDataPoints, { 'cardType': 'P' }), 'cardHeaderId');
            let surCardDataArr = _.map(surfaceDataGrp, surfaceDataGrp.value);
            let pumpCardDataArr = _.map(pumpCardDataGrp, pumpCardDataGrp.value);
            this.surCrdSvgGrp.selectAll("path").remove();
            this.pumpCrdSvgGrp.selectAll("path").remove();

            for (let ci in surCardDataArr) {
                let surCardData = surCardDataArr[ci];
                let pumpCardData = pumpCardDataArr[ci];
                setTimeout(() => {
                    this.renderCard(ci, surCardData, pumpCardData);
                }, +ci * this.totalAnimationTime);
            }

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


        public createDropDown(argDropDownType: string) {
            let ddDiv = document.createElement("div");
            let ddLabel: HTMLElement;
            ddDiv.setAttribute("class", "col-xs-3 input-group");
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
                if (argDropDownType == DataColumns.pumpId) {
                    this.pumpSelVal = selVal;
                    this.resetOtherControls();
                }
                else if (argDropDownType == DataColumns.eventId) this.eventSelVal = selVal;

                this.animateGraph(this.updateGraphData());
            }

            ddDiv.appendChild(labelDiv);
            ddDiv.appendChild(dropDown);
            return ddDiv;
        }

        private resetOtherControls() {
            $("#" + DataColumns.eventId).val('all');
            $("#" + DataColumns.startDate).val('');
            $("#" + DataColumns.endDate).val('');
            this.eventSelVal = 'all';
        }

        public updateGraphData(): DataPoint[] {
            let retGraphDataSet: DataPoint[] = _.sortBy(this.dataSet.dataPoints, 'cardId');

            if (this.pumpSelVal != 'all') retGraphDataSet = _.filter(this.dataSet.dataPoints, { 'pumpId': +this.pumpSelVal });
            let startDateTime = new Date(String($("#" + DataColumns.startDate).val())).getTime() / 1000;
            let endDateTime = new Date(String($("#" + DataColumns.endDate).val())).getTime() / 1000;

            if (!isNaN(startDateTime) && !isNaN(endDateTime)) {
                retGraphDataSet = _.filter(this.dataSet.dataPoints, (d) => {
                    if (d.epocDate >= startDateTime && d.epocDate <= endDateTime) {
                        return true;
                    }
                });
            }

            if (this.eventSelVal != 'all') retGraphDataSet = _.filter(retGraphDataSet, { 'eventId': +this.eventSelVal });

            return retGraphDataSet;
        }

    }

}