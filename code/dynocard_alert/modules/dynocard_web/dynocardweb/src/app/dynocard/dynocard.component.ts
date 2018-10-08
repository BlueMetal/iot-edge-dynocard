import { Component, ViewEncapsulation, OnInit } from '@angular/core';

import { DataService, UrlManagingService } from '../../services';
import { ViewChild, ElementRef } from '@angular/core';

import * as d3 from 'd3';
import * as _ from 'lodash';

// Just import through angular.json "scripts", rather than trying to import here
declare var $;
// import * as $ from 'jquery/dist/jquery.min.js';
// import 'eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js';

// Interfaces
export interface ViewModel {
  dataPoints: DataPoint[];
  maxValue: number;
}

export interface DataPoint {
  pumpId: number;
  eventId: number;
  cardHeaderId: number;
  epocDate: number;
  cardType: string;
  cardId: number;
  position: number;
  load: number;
};

export interface IViewport {
  height: number;
  width: number;
}

enum VisualUpdateType {
  Data = 2,
  Resize = 4,
  ViewMode = 8,
  Style = 16,
  ResizeEnd = 32,
  All = 62,
}

const enum ViewMode {
  View = 0,
  Edit = 1,
  InFocusEdit = 2,
}

const enum EditMode {
  /** Default editing mode for the visual. */
  Default = 0,
  /** Indicates the user has asked the visual to display advanced editing controls. */
  Advanced = 1,
}

export interface VisualUpdateOptions {
  viewport: IViewport;
  dataViews: DataView[];
  type: VisualUpdateType;
  viewMode?: ViewMode;
  editMode?: EditMode;
}

// This is an array of column names from the DataSet, the spelling must match exactly
export class DataColumns {
  static pumpId = 'pump_ID';
  static eventId = 'event_ID';
  static cardHeaderId = 'cardHeader_ID';
  static epocDate = 'epoC_DATE';
  static startDate = 'startDate';
  static endDate = 'endDate';
  static cardType = 'card_Type';
  static cardId = 'card_ID';
  static position = 'position';
  static load = 'load';
}

@Component({
  selector: 'dynocard-chart',
  templateUrl: './dynocard.component.html',
  styleUrls: ['./dynocard.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class DynoCardComponent implements OnInit {
  chartData: DataPoint[];
  @ViewChild('controls') controls: ElementRef;
  //---Power Bi
  private target: HTMLElement;
  private refoptions: VisualUpdateOptions;

  // --- SVG
  private dynoCardSvg: d3.Selection<SVGAElement>;
  private surCrdSvgGrp: d3.Selection<SVGAElement>;
  private pumpCrdSvgGrp: d3.Selection<SVGAElement>;
  private drawLineFunc: d3.svg.Line<DataPoint>;
  private svgCanvasWidth: number;
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
  private margin = { top: 150, right: 100, bottom: -100, left: 0 };
  private totalAnimationTime: number = 2000;
  public errorMessage: any;

  constructor(private dataService: DataService, private urlManagingService: UrlManagingService) {
    this.svgCanvasWidth = 1200;
    this.svgCanvasHeight = 560;
  }

  ngOnInit() {
    this.loadChartData();
  }

  async loadChartData() {
    await this.dataService.get(this.urlManagingService.getDynoCardData).toPromise()

      .then((response) => {
        this.chartData = response;
        this.controls.nativeElement.appendChild(this.createInitialHeader());
        const animateButton = this.createAnimationButton();
        document.getElementById('buttonDiv').appendChild(animateButton);

        this.renderChart();
      })

      .catch(error => {
        console.log('DynoCardComponent.loadChartData() Error');
        this.errorMessage = error;
        return error;
      });
  }

  private async renderChart() {

    this.dynoCardSvg = d3.select('svg');

    this.surCrdSvgGrp = this.dynoCardSvg.append('g').classed('sur-svg-grp-cls', true);
    this.surCrdSvgGrp.attr({ id: 'surfaceCard' });
    this.pumpCrdSvgGrp = this.dynoCardSvg.append('g').classed('pump-svg-grp-cls', true);
    this.pumpCrdSvgGrp.attr({ id: 'pumpCard' });

    this.xAxisGroup = this.dynoCardSvg.append('g').classed('x-axis', true);
    this.yAxisGroupSurface = this.dynoCardSvg.append('g').classed('y-axis', true);
    this.yAxisGroupPump = this.dynoCardSvg.append('g').classed('y-axis-pump', true);


    this.dataSet = await this.getTableData();
    // console.log('this.getTableData(): ', this.dataSet)

    this.dynoCardSvg.attr({
      width: this.svgCanvasWidth,
      height: this.svgCanvasHeight = this.svgCanvasHeight - this.margin.top - this.margin.bottom
    });

    const childNodes = document.getElementById('controlDiv');
    while (childNodes.firstChild) {
      childNodes.removeChild(childNodes.firstChild);
    }

    // create this in angular
    const pumpDD = this.createDropDown(DataColumns.pumpId);
    const eventDD = this.createDropDown(DataColumns.eventId);
    const startDatePicker = this.createDateTimePicker(DataColumns.startDate);
    const endDatePicker = this.createDateTimePicker(DataColumns.endDate);
    document.getElementById('controlDiv').appendChild(pumpDD);
    document.getElementById('controlDiv').appendChild(startDatePicker);
    document.getElementById('controlDiv').appendChild(endDatePicker);
    document.getElementById('controlDiv').appendChild(eventDD);

    this.dynoCardSvg.selectAll('line').remove();
    this.dynoCardSvg.append('line').attr({
      x1: this.margin.right,
      y1: this.svgCanvasHeight / 2,
      x2: this.svgCanvasWidth,
      y2: this.svgCanvasHeight / 2,
      'stroke-width': 0.5,
      'stroke': 'gray'
    });

    // Convert the string to a number, so .max can get the correct value. it should but a number but for some reason .max isn't reading it as a number, so force it to.
    this.dataSet.dataPoints.forEach(d => {
      d.position = parseInt(<any>d.position, 10);
    });

    //--- Define X & Y  Axis Scale and Line
    const xMax = d3.max(this.dataSet.dataPoints, d => d.position);
    this.xAxis_Position = d3.scale.linear().domain([-1, xMax]).range([0, this.svgCanvasWidth - 100]);
    this.yAxis_Load = d3.scale.linear().domain(d3.extent(this.dataSet.dataPoints, d => d.load)).range([this.svgCanvasHeight / 2, 0]);

    // Draw X axis line
    const xAxisLine = d3.svg.axis().scale(this.xAxis_Position).orient('bottom').tickSize(5).tickFormat(d => d + ' in');
    this.xAxisGroup.call(xAxisLine).attr({
      transform: 'translate(' + this.margin.right + ', ' + (this.svgCanvasHeight - 20) + ')'
    });

    // Draw Y axis line
    const yAxisLine = d3.svg.axis().scale(this.yAxis_Load).orient('left').tickSize(5).tickFormat(d =>
      Number(d) / 1000 + ' klb');
    this.yAxisGroupSurface.call(yAxisLine).attr({
      transform: 'translate(' + this.margin.right + ', 5)'
    });
    this.yAxisGroupPump.call(yAxisLine).attr({
      transform: 'translate(' + this.margin.right + ', ' + (this.svgCanvasHeight / 2) + ')'
    });

    //-- Define Path Draw function
    this.drawLineFunc = d3.svg.line<DataPoint>().interpolate('basis')
      .x((dp: DataPoint) => {
        return this.xAxis_Position(dp.position);
      })
      .y((dp: DataPoint) => {
        return this.yAxis_Load(dp.load);
      });

    // For some reason, the first time the animateGraph() method is called, the surfacePumpCard does not render properly, and cuts off the right end of the lines. If animateGraph() is called a second time, then it renders fine, so this accomplishes that. Not ideal but it works.
    this.animateGraph(this.updateGraphData());
    setTimeout(function () {
      this.animateGraph(this.updateGraphData());
    }.bind(this), 2000);

  }

  private animateGraph(argGraphDataSet: DataPoint[]
  ) {
    const allDataPoints = _.sortBy(argGraphDataSet, 'cardId');
    const surfaceDataGrp = _.groupBy(_.filter(allDataPoints, { 'cardType': 'S' }), 'cardHeaderId');
    const pumpCardDataGrp = _.groupBy(_.filter(allDataPoints, { 'cardType': 'P' }), 'cardHeaderId');
    const surCardDataArr = _.map(surfaceDataGrp, surfaceDataGrp.value);
    const pumpCardDataArr = _.map(pumpCardDataGrp, pumpCardDataGrp.value);
    this.surCrdSvgGrp.selectAll('path').remove();
    this.pumpCrdSvgGrp.selectAll('path').remove();

    const self = this;
    for (const ci in surCardDataArr) {
      const surCardData = surCardDataArr[ci];
      const pumpCardData = pumpCardDataArr[ci];
      setTimeout(() => {
        // console.log('rendering:', ci);
        this.renderCard(ci, surCardData, pumpCardData);
      }, +ci * self.totalAnimationTime + 2000);
    }
  }

  public updateGraphData(): DataPoint[] {
    let retGraphDataSet: DataPoint[] = _.sortBy(this.dataSet.dataPoints, 'cardId');

    if (this.pumpSelVal !== 'all') retGraphDataSet = _.filter(this.dataSet.dataPoints, { 'pumpId': +this.pumpSelVal });
    const startDateTime = new Date(String($('#' + DataColumns.startDate).val())).getTime() / 1000;
    const endDateTime = new Date(String($('#' + DataColumns.endDate).val())).getTime() / 1000;

    if (!isNaN(startDateTime) && !isNaN(endDateTime)) {
      retGraphDataSet = _.filter(this.dataSet.dataPoints, (d) => {
        if (d.epocDate >= startDateTime && d.epocDate <= endDateTime) {
          return true;
        }
      });
    }

    if (this.eventSelVal !== 'all') retGraphDataSet = _.filter(retGraphDataSet, { 'eventId': +this.eventSelVal });

    return retGraphDataSet;
  }

  private getTableData(): Promise<ViewModel> {
    return new Promise((resolve, reject) => {

      const dataPoints: DataPoint[] = [];
      // let jsonData: DataPoint[] = [];
      let retDataView: ViewModel;
      const dataView = this.chartData;

      retDataView = {
        dataPoints: dataPoints,
        maxValue: d3.max(this.chartData, d => d.load)
      };

      for (let i = 0; i < dataView.length; i++) {
        retDataView.dataPoints.push({
          pumpId: <number>+dataView[i][DataColumns.pumpId],
          eventId: <number>+dataView[i][DataColumns.eventId],
          cardHeaderId: <number> +dataView[i][DataColumns.cardHeaderId],
          epocDate: <number>+dataView[i][DataColumns.epocDate],
          cardType: <string>dataView[i][DataColumns.cardType],
          cardId: <number>dataView[i][DataColumns.cardId],
          position: <number>dataView[i][DataColumns.position],
          load: <number>+dataView[i][DataColumns.load]
        });
      }

      // console.log(retDataView);
      resolve(retDataView);

      // // Load from CSV
      // d3.csv("assets/dataset1.csv")
      // // .row(this.rowConversion) // doesn't seem to need a row conversion function to work
      //   .get(function (error, data: DataPoint[]) {
      //     if (error) reject(error);
      //
      //     jsonData = data;
      //     retDataView = {
      //       dataPoints: dataPoints,
      //       maxValue: d3.max(data, d => d.load)
      //     }
      //
      //     const dataView = jsonData;
      //
      //     for (let i = 0; i < dataView.length; i++) {
      //       retDataView.dataPoints.push({
      //         pumpId: <number>+dataView[i][DataColumns.pumpId],
      //         eventId: <number>+dataView[i][DataColumns.eventId],
      //         cardHeaderId: <number> +dataView[i][DataColumns.cardHeaderId],
      //         epocDate: <number>+dataView[i][DataColumns.epocDate],
      //         cardType: <string>dataView[i][DataColumns.cardType],
      //         cardId: <number>dataView[i][DataColumns.cardId],
      //         position: <number>dataView[i][DataColumns.position],
      //         load: <number>+dataView[i][DataColumns.load]
      //       });
      //     }
      //
      //     // console.log(retDataView);
      //     resolve(retDataView);
      //
      //   }.bind(this))

    });
  }

  private renderCard(ci, surCardData, pumpCardData) {
    const plotSurfacePath = this.surCrdSvgGrp.selectAll('path' + ci).data([surCardData]);
    plotSurfacePath.enter().append('path').classed('path-cls', true);
    plotSurfacePath.exit().remove();
    plotSurfacePath.attr('stroke', 'steelblue')
      .attr('stroke-width', 2)
      .attr('fill', 'none')
      .attr('d', this.drawLineFunc);
    this.plotteSurfacedPath = d3.select(document.getElementById('surfaceCard')).selectAll('path');
    const surfacePathLength = this.plotteSurfacedPath.node().getTotalLength();

    plotSurfacePath
      .attr('stroke-dasharray', surfacePathLength + ' ' + surfacePathLength)
      .attr('stroke-dashoffset', surfacePathLength)
      .transition()
      .duration(2000)
      .ease('linear')
      .attr('stroke-dashoffset', 0);

    const plotPumpPath = this.pumpCrdSvgGrp.selectAll('path' + ci).data([pumpCardData]);
    plotPumpPath.enter().append('path').classed('path-cls', true);
    plotPumpPath.exit().remove();
    plotPumpPath.attr('stroke', 'brown')
      .attr('stroke-width', 2)
      .attr('fill', 'none')
      .attr('d', this.drawLineFunc);

    this.plottePumpPath = d3.select(document.getElementById('pumpCard')).selectAll('path');
    const pumpPathLength = this.plottePumpPath.node().getTotalLength();

    plotPumpPath
      .attr('stroke-dasharray', pumpPathLength + ' ' + pumpPathLength)
      .attr('stroke-dashoffset', pumpPathLength)
      .transition()
      .duration(2000)
      .ease('linear')
      .attr('stroke-dashoffset', 0);
    this.surCrdSvgGrp.attr({
      transform: 'translate(10,0)'
    });
    this.surCrdSvgGrp.attr({
      transform: 'translate(' + this.margin.right + ',0)'
    });
    this.pumpCrdSvgGrp.attr({
      transform: 'translate(' + this.margin.right + ',' + (this.svgCanvasHeight / 2 - 30) + ')'
    });
  }

  private createInitialHeader() {
    const baseDiv: HTMLElement = document.createElement('div');
    baseDiv.setAttribute('class', 'container-fluid');

    const reportTitle: HTMLElement = document.createElement('p');
    reportTitle.setAttribute('class', 'text-center');
    // reportTitle.appendChild(document.createTextNode(" Graph: Dyno Card"));
    baseDiv.appendChild(reportTitle);

    const controlDivRow: HTMLElement = document.createElement('div');
    controlDivRow.setAttribute('class', 'form-inline');
    const controlDiv: HTMLElement = document.createElement('div');
    controlDiv.setAttribute('id', 'controlDiv');
    controlDiv.setAttribute('class', 'row');
    controlDivRow.appendChild(controlDiv);
    baseDiv.appendChild(controlDivRow);

    const dynoCardDiv: HTMLElement = document.createElement('div');
    dynoCardDiv.setAttribute('id', 'dynoCardDiv');
    dynoCardDiv.setAttribute('class', 'row');
    baseDiv.appendChild(dynoCardDiv);

    baseDiv.appendChild(document.createElement('hr'));
    const buttonDiv: HTMLElement = document.createElement('div');
    buttonDiv.setAttribute('id', 'buttonDiv');
    buttonDiv.setAttribute('class', 'row');
    baseDiv.appendChild(buttonDiv);

    return baseDiv;
  }

  private createDateTimePicker(argDateType) {

    const ddDiv = document.createElement('div');
    ddDiv.setAttribute('class', 'col-xs-3 form-group');
    ddDiv.setAttribute('id', 'datePicker1');
    const dateDiv = document.createElement('div');
    dateDiv.setAttribute('class', 'input-group');
    dateDiv.setAttribute('id', argDateType + 'Picker');

    const dateInput = document.createElement('input');
    dateInput.setAttribute('class', 'form-control');
    dateInput.setAttribute('type', 'text');
    dateInput.setAttribute('id', argDateType);
    dateInput.onchange = () => {
      this.rerenderEventDropDown();
    };

    const spanOuter = document.createElement('span');
    spanOuter.setAttribute('class', 'input-group-addon');
    const spanIcon = document.createElement('span');
    spanIcon.setAttribute('class', 'glyphicon glyphicon-calendar');
    spanOuter.appendChild(spanIcon);

    if (argDateType === DataColumns.startDate) {
      dateInput.setAttribute('placeholder', 'Start Date');
      dateInput.setAttribute('value', '04/08/1996 12:00 AM');
    }
    else {
      dateInput.setAttribute('placeholder', 'End Date');
      dateInput.setAttribute('value', '04/08/2019 12:00 AM');

    }

    spanOuter.onmouseover = (event: Event) => {
      $('#' + argDateType + 'Picker').datetimepicker();
    };
    spanOuter.onclick = () => {
      this.rerenderEventDropDown();
    };

    dateDiv.appendChild(dateInput);
    dateDiv.appendChild(spanOuter);
    ddDiv.appendChild(dateDiv);

    return ddDiv;
  }

  private createAnimationButton() {
    const animationButton = document.createElement('button');
    animationButton.setAttribute('type', 'button');
    animationButton.setAttribute('class', 'btn btn-success center-block');
    animationButton.textContent = 'Run DynoCard Animation';
    animationButton.onclick = function () {
      this.animateGraph(this.updateGraphData());
    }.bind(this);
    return animationButton;
  }

  public rerenderEventDropDown() {
    const eventDD = document.getElementById(DataColumns.eventId);
    while (eventDD.firstChild) {
      eventDD.removeChild(eventDD.firstChild);
    }
    this.eventSelVal = 'all';
    const updatedDataSet = this.updateGraphData();
    const tmpEventDDList = _.uniq(_.map(updatedDataSet, 'eventId'));

    const allOp = document.createElement('option');
    if (tmpEventDDList.length > 0) {
      allOp.text = 'All';
      allOp.value = 'all';
    } else {
      allOp.text = 'No Event';
      allOp.value = 'No';
    }

    eventDD.appendChild(allOp);
    for (let i = 0; i < tmpEventDDList.length; i++) {
      const option = document.createElement('option');
      option.value = String(tmpEventDDList[i]);
      option.text = String(tmpEventDDList[i]);
      eventDD.appendChild(option);
    }
  }

  public createDropDown(argDropDownType: string) {
    const ddDiv = document.createElement('div');
    // let ddLabel: HTMLElement;
    ddDiv.setAttribute('class', 'col-xs-3 input-group');
    const labelDiv = document.createElement('div');
    labelDiv.setAttribute('class', 'input-group-addon');
    const dropDown = document.createElement('select');
    dropDown.setAttribute('class', 'form-control');
    dropDown.setAttribute('id', argDropDownType);
    let dropDownData = [];

    if (argDropDownType === DataColumns.pumpId) {
      labelDiv.appendChild(document.createTextNode('Pump: '));
      const pumpIdList = _.uniq(_.map(this.dataSet.dataPoints, 'pumpId'));
      dropDownData = _.map(pumpIdList, item => String(item));
    } else if (argDropDownType === DataColumns.cardType) {
      labelDiv.appendChild(document.createTextNode('Card Type'));
      dropDownData = _.uniq(_.map(this.dataSet.dataPoints, 'cardType'));
      this.cardTypeDDList = dropDownData;
    } else if (argDropDownType === DataColumns.eventId) {
      labelDiv.appendChild(document.createTextNode('Event:  '));
      dropDownData = _.uniq(_.map(this.dataSet.dataPoints, 'eventId'));
      this.eventIdDDList = dropDownData;
    }

    //Create and append the options
    const allOp = document.createElement('option');
    allOp.text = 'All';
    allOp.value = 'all';
    dropDown.appendChild(allOp);
    for (let i = 0; i < dropDownData.length; i++) {
      const option = document.createElement('option');
      option.value = dropDownData[i];
      option.text = dropDownData[i];
      dropDown.appendChild(option);
    }
    // dropDown.ondblclick=()=>{
    //     if (argDropDownType == DataColumns.eventId){
    //         this.rerenderEventDropDown();
    //     }
    // }
    dropDown.onchange = (event: Event) => {
      const selVal = $('#' + argDropDownType).val();
      if (argDropDownType === DataColumns.pumpId) {
        this.pumpSelVal = selVal;
        this.resetOtherControls();
      }
      else if (argDropDownType === DataColumns.eventId) this.eventSelVal = selVal;


      this.animateGraph(this.updateGraphData());
    };

    ddDiv.appendChild(labelDiv);
    ddDiv.appendChild(dropDown);
    return ddDiv;
  }

  private resetOtherControls() {
    $('#' + DataColumns.eventId).val('all');
    // $("#" + DataColumns.startDate).val('');
    // $("#" + DataColumns.endDate).val('');
    this.eventSelVal = 'all';
  }


}
