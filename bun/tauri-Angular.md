```
bun create tauri-app@latest

Project name: my-tauri-app (원하는 이름)
Choose which language to use for your frontend: TypeScript / JavaScript
Choose your package manager: bun
Choose your UI template: Angular

cd my-tauri-app
bun install

src-tauri/capabilities/default.json

{
  "$schema": "../gen/schemas/desktop-capability.json",
  "identifier": "default",
  "description": "default capability",
  "windows": [
    "main"
  ],
  "permissions": [
    "core:default",
    "core:window:allow-start-dragging",
    "core:window:allow-minimize",
    "core:window:allow-maximize",
    "core:window:allow-toggle-maximize",
    "core:window:allow-close"
  ]
}

src-tauri/tauri.conf.json

"windows": [
  {
    "title": "My App",
    "width": 800,
    "height": 600,
    "decorations": false,   
    "transparent": false,   
    "fullscreen": false,
    "resizable": true
  }
]

src/styles.css

:root {
  color-scheme: light !important;
}

html, body {
  background-color: #ffffff !important;
  color: #333333 !important;
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

app-root {
  display: block;
  padding-top: 32px; 
}


src/app/titlebar.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-titlebar',
  standalone: true,
  template: `
    <div class="titlebar" data-tauri-drag-region>
      <div class="title">My App</div>
      <div class="window-controls">
        <button class="control-btn minimize" (click)="minimize($event)">➖</button>
        <button class="control-btn maximize" (click)="maximize($event)">🔳</button>
        <button class="control-btn close" (click)="close($event)">❌</button>
      </div>
    </div>
  `,
  styles: [`
    .titlebar {
      height: 32px;
      background: #f3f3f3;
      color: #333333;
      display: flex;
      justify-content: space-between;
      align-items: center;
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      padding: 0 10px;
      user-select: none;
      z-index: 9999;
      border-bottom: 1px solid #e0e0e0;
    }
    .title {
      font-size: 12px;
      pointer-events: none;
    }
    .window-controls {
      display: flex;
      gap: 5px;
    }
    .control-btn {
      background: transparent;
      border: none;
      width: 28px;
      height: 28px;
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 10px;
      color: #333333;
      border-radius: 4px;
      -webkit-app-region: no-drag; /* 🎯 드래그 영역과 버튼 클릭 영역을 확실히 분리 */
    }
    .control-btn:hover {
      background: rgba(0, 0, 0, 0.1);
    }
    .control-btn.close:hover {
      background: #ff4d4f;
      color: white;
    }
  `]
})
export class TitlebarComponent {

  // 🎯 @tauri-apps/api 정적 임포트 에러를 완전히 우회하는 v2 윈도우 제어 방식
  private async getWindow() {
    const { getCurrentWindow } = await import('@tauri-apps/api/window');
    return getCurrentWindow();
  }

  async minimize(event: MouseEvent) {
    event.stopPropagation();
    const win = await this.getWindow();
    await win.minimize();
  }

  async maximize(event: MouseEvent) {
    event.stopPropagation();
    const win = await this.getWindow();
    await win.toggleMaximize(); // 최대화/이전크기 복원 토글
  }

  async close(event: MouseEvent) {
    event.stopPropagation();
    const win = await this.getWindow();
    await win.close();
  }
}


src/app/app.component.ts

import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { invoke } from '@tauri-apps/api/core'; // 🎯 Tauri 백엔드 통신용 함수
import { TitlebarComponent } from './titlebar'; // 🎯 커스텀 타이틀바

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TitlebarComponent], // 🎯 타이틀바 컴포넌트 등록
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'my-tauri-app';
  
  // 🎯 HTML이 애타게 찾던 변수와 함수를 다시 정의해 줍니다.
  greetingMessage = "";

  greet(event: SubmitEvent, name: string): void {
    event.preventDefault();
    // Tauri Rust 백엔드의 'greet' 커맨드를 호출합니다.
    invoke<string>("greet", { name }).then((text) => {
      this.greetingMessage = text;
    });
  }
}

src/app/app.component.html

<!-- 최상단에 커스텀 타이틀바 삽입 -->
<app-titlebar></app-titlebar>

<!-- 기존 Angular 앱 영역 -->
<router-outlet></router-outlet>
```
