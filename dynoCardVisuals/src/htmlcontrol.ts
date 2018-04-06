module powerbi.extensibility.visual {
    export class HtmlControl{

        public static createInitialHeader() {
            const baseDiv: HTMLElement = document.createElement("div");
            baseDiv.setAttribute("class", "container-fluid");

            const reportTitle: HTMLElement = document.createElement("p");
            reportTitle.setAttribute("class", "text-center")
            reportTitle.appendChild(document.createTextNode(" Graph: Dyno Card"));
            baseDiv.appendChild(reportTitle);

            const controlDivRow: HTMLElement = document.createElement("div");
            controlDivRow.setAttribute("class", "form-inline");
            const controlDiv: HTMLElement = document.createElement("div");
            controlDiv.setAttribute("id", "controlDiv");
            controlDiv.setAttribute("class", "row");
            controlDivRow.appendChild(controlDiv);
            baseDiv.appendChild(controlDivRow);

            const dynoCardDiv: HTMLElement = document.createElement("div");
            dynoCardDiv.setAttribute("id", "dynoCardDiv");
            dynoCardDiv.setAttribute("class", "row");
            baseDiv.appendChild(dynoCardDiv);

            baseDiv.appendChild(document.createElement("hr"));
            const buttonDiv: HTMLElement = document.createElement("div");
            buttonDiv.setAttribute("id", "buttonDiv");
            buttonDiv.setAttribute("class", "row");
            baseDiv.appendChild(buttonDiv);

            return baseDiv;
        }

        public static createDateTimePicker(argDateType) {

            let ddDiv = document.createElement("div");
            ddDiv.setAttribute("class", "col-xs-3 form-group");
            ddDiv.setAttribute("id", "datePicker1");
            let dateDiv = document.createElement("div");
            dateDiv.setAttribute("class","input-group");
            dateDiv.setAttribute("id",argDateType+"Picker");
            
            let dateInput = document.createElement("input");
            dateInput.setAttribute("class", "form-control");
            dateInput.setAttribute("type", "text");
            dateInput.setAttribute("id",argDateType);
        
            let spanOuter =document.createElement("span");
            spanOuter.setAttribute("class","input-group-addon");
            let spanIcon =document.createElement("span");
            spanIcon.setAttribute("class","glyphicon glyphicon-calendar");
            spanOuter.appendChild(spanIcon);

            if(argDateType==DataColumns.startDate)  dateInput.setAttribute("placeholder","Start Date");
            else dateInput.setAttribute("placeholder","End Date");

            
            spanOuter.onmouseover= (event: Event) => {
                $('#'+argDateType+"Picker").datetimepicker();
            }

            dateDiv.appendChild(dateInput);
            dateDiv.appendChild(spanOuter);
            ddDiv.appendChild(dateDiv);

            return ddDiv;
        }

        public static createAnimationButton(argRef) {
            let animationButton = document.createElement("button");
            animationButton.setAttribute("type", "button");
            animationButton.setAttribute("class", "btn btn-success center-block");
            animationButton.textContent = "Run DynoCard Animation";
            animationButton.onclick = function () {

                console.log("Final DataSET: ", argRef.updateGraphData());
                
                argRef.animateGraph(argRef.updateGraphData());
            }
            return animationButton;
        }


    }
}