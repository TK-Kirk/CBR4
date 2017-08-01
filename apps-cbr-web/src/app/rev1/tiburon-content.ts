import {Component, DoCheck } from '@angular/core';

import postscribe from 'postscribe';

@Component({
    selector: 'tiburon-content',
    template: ''
})
export class TiburonContentComponent implements DoCheck  {

    ngDoCheck() {
      postscribe('#tiburonContent', '<script src="http://ldsapi.tmginteractive.com/generateplacementscript.aspx?placement=48999100&publisher=572055&affid=&subid=&redirect="><\/script>');
    }
}
