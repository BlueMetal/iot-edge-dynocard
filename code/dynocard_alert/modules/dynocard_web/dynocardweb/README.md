# IotEdgeDynocard

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 6.2.3.

## Pushing to Azure

This project is pushed to an Azure App Service located at [https://iotdynocardmoxa-web.azurewebsites.net](https://iotdynocardmoxa-web.azurewebsites.net/). The compiled project is pushed using git subtrees.

The specific command to push the DynoCard webapp is:
- `git remote add azure https://dynocard@iotdynocardmoxa-web.scm.azurewebsites.net:443/iotdynocardmoxa-web.git`
- `git subtree push --prefix dist/iot-edge-dynocard-web azure master`

If running into issues merging Git Subtrees, See:
- https://stackoverflow.com/questions/33172857/how-do-i-force-a-subtree-push-to-overwrite-remote-changes
- https://community.atlassian.com/t5/Questions/How-can-one-eliminate-subtree-commit-history-due-to-a-remote/qaq-p/76357

The command syntax to create and manage a git subtree is:

- `git subtree add —prefix [path_to_folder]`
- `git subtree pull —prefix [path_to_folder] [remote] [remote-branch]`
- `git subtree push —prefix [path_to_folder] [remote] [remote-branch]`

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
