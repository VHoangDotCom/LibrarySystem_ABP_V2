import { LibrarySystemTemplatePage } from './app.po';

describe('LibrarySystem App', function() {
  let page: LibrarySystemTemplatePage;

  beforeEach(() => {
    page = new LibrarySystemTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
