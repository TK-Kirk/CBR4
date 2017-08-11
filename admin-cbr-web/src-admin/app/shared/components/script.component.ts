import { Component, ElementRef, ViewChild, Input, OnInit } from '@angular/core';

@Component({
    selector: 'script-hack',
    templateUrl: './script.component.html'
})
export class ScriptComponent implements OnInit {

    @Input()
    src: string;

    @Input()
    type: string;

    @ViewChild('script') script: ElementRef;

    convertToScript() {
        const element = this.script.nativeElement;
        const script = document.createElement('script');
        script.type = this.type ? this.type : 'text/javascript';
        if (this.src) {
            script.src = this.src;
        }
        if (element.innerHTML) {
            script.innerHTML = element.innerHTML;
        }
        const parent = element.parentElement;
        parent.parentElement.replaceChild(script, parent);
    }

    public ngOnInit(): void {
        this.convertToScript();
    }
}

