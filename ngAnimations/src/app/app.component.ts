import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shake, shakeX, tada } from 'ng-animate';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [
    trigger("shake", [
      transition(":increment", useAnimation(shakeX, { params: { timing: 2 } })),
    ]),
    trigger("bounce", [
      transition(":increment", useAnimation(bounce, { params: { timing: 4 } })),
    ]),
    trigger("tada", [
      transition(":increment", useAnimation(tada, { params: { timing: 3 } })),
    ])
  ],
  standalone: true
})
export class AppComponent {
  title = 'ngAnimations';
  ng_shake = 0;
  ng_bounce = 0;
  ng_tada = 0;
  rotate = false;
  constructor() {
  }


   shakeFlipAndBounce_Angular(loop : boolean = false) {

    // Rouge immédiatement
    this.ng_shake++;

    // Vert après 2 sec
    setTimeout(() => {
      this.ng_bounce++;
    }, 2000);// settimeout 2000, veux dire 2sec apres avoir commencer la premiere animation.

    // Bleu 1 sec avant la fin du vert
    // Vert dure 4 sec et commence à 2
    // donc finit à 6
    // bleu commence à 5
    setTimeout(() => {
      this.ng_tada++;
    }, 5000);

    if(loop){
        setTimeout(() => {
      this.shakeFlipAndBounce_Angular(true);
    }, 8000);
    }
  }


  tourner(){
    this.rotate = true;
setTimeout(() => {
      this.rotate = false;
    }, 2000);
  }


}
