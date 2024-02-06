import { CustomDataPipe } from './custom-data.pipe';

describe('CustomDataPipe', () => {
  it('create an instance', () => {
    const pipe = new CustomDataPipe();
    expect(pipe).toBeTruthy();
  });
});
