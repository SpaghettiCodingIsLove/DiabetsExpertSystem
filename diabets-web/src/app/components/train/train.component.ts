import { Component, OnInit } from '@angular/core';
import { DiabetsApiService } from 'src/app/services/diabets-api.service';

@Component({
  selector: 'app-train',
  templateUrl: './train.component.html',
  styleUrls: ['./train.component.scss']
})
export class TrainComponent implements OnInit {
  public trained: boolean = false;
  public score: number = 0;
  public file?: Blob;

  constructor(private apiService: DiabetsApiService) { }

  ngOnInit(): void {
  }

  fileChanged(e: Event) {
    this.file = (<HTMLInputElement>e.target)?.files?.[0];
  }

  public train() {
    let fileReader = new FileReader();

    fileReader.onload = (e) => {
      if (fileReader.result !== null)
      {
        this.apiService.train(fileReader.result as string)
          .subscribe(x => { this.score = x.score; this.trained = true });
      }
    }

    if (this.file !== undefined) {
      fileReader.readAsText(this.file);
    }    
  }
}
