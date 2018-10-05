import { AppPage } from './app.po';

describe('IOT Edge DynoCard App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should have a dynocard-chart element', () => {
    page.navigateTo();
    // expect(page.getParagraphText()).toEqual('hi');
    expect(page.getDynoCardElement().isPresent()).toBeTruthy();
  });
});
