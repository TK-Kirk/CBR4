import { AppsCbrWebPage } from './app.po';

describe('apps-cbr-web App', () => {
  let page: AppsCbrWebPage;

  beforeEach(() => {
    page = new AppsCbrWebPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
