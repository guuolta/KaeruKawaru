# かわるがわるかえる
## 目次
- [かわるがわるかえる](#かわるがわるかえる)
  - [目次](#目次)
  - [ゲーム内容](#ゲーム内容)
  - [制作経緯](#制作経緯)
  - [こだわりポイント](#こだわりポイント)
  - [制作メンバー](#制作メンバー)
  - [制作環境](#制作環境)
  - [使用技術](#使用技術)
  - [担当箇所](#担当箇所)
  - [ソースファイルのディレクトリ構成](#ソースファイルのディレクトリ構成)
  - [ゲームのURL](#ゲームのurl)
## ゲーム内容
かえるや卵をクリックしてお題を完成させるゲームです。  
卵、オタマジャクシ、かえるというループで変化していきます。  
お題を完成させるまでのクリック数やお題の同時クリアなどでボーナスが入ります。
## 制作経緯
このゲームは、2024年3月18日に開催されたUnityRoom主催のUnityOneWeekに参加した際のゲームです。  
UnityOneWeekは、一週間でゲームを開発するイベントで、今回参加したUnityOneWeekでのお題は、「かわる」でした。  
このゲームは、同じサークルのメンバーと制作しました。
## こだわりポイント
UIの設計にこだわっており、一つの画面ごとにデータを扱うモデル、見た目を扱うビュー、ビューとモデルを繋ぐプレゼンターのクラスを作成し、変更に強い設計を心掛けました。
また、お題をクリアしたマスに演出をつけ、どこのマスがお題をクリアしたのかわかりやすいようにこだわりました。
お題のクリアを判定するアルゴリズムもこだわっています。ただマスの全探索をすると計算量が多くなってしまうので、2*2のお題の左上の部分がマスの右から２番目に一致しない時は、一番右があっているか判定しないようにするなど、計算路湯を小さくする工夫をしました。
## 制作メンバー
- プログラマー：2人
- 3Dモデレーター：1人
- サウンドクリエイター：1人
## 制作環境
Unity: 2022.3.15f1
## 使用技術
- UniRx
- DOTweenPro
- UniTask
## 担当箇所
- 基底クラス
- アウトゲーム
  - タイトル画面
  - タイトルのカメラ遷移
  - ランキング機能
- インゲーム
  - お題の出題
  - お題の判定
  - お題のUI
  - カエルのオブジェクト切り替え
  - プレイヤーの操作
  - ポーズ機能、UI
  - タイマー機能、UI
  - リザルト機能、UI
- 素材
  - タイトルロゴ
  - ボタン
  - 遊び方説明スライド
  - パネル背景
## ソースファイルのディレクトリ構成
```
0_coading.
│   
├───0_Base (基底クラス)
│   │   DontDestroySingletonObject.cs
│   │   ObjectBase.cs
│   │   SingletonObjectBase.cs
│   │   
│   ├───GameObject
│   │       GameObjectBase.cs
│   │       
│   └───UI
│       │   PresenterBase.cs
│       │   UIBase.cs
│       │   ViewBase.cs
│       │   
│       ├───Panel
│       │   │   PanelManagerBase.cs
│       │   │   PanelPresenterBase.cs
│       │   │   PanelViewBase.cs
│       │   │   
│       │   ├───Select
│       │   │       SelectPanelPresenterBase.cs
│       │   │       SelectPanelViewBase.cs
│       │   │       
│       │   └───Sound
│       │           SoundPanelPresenterBase.cs
│       │           
│       └───Parts
│           │   AnimationPartBase.cs
│           │   UIFadeAnimationUIPartBase.cs
│           │   
│           ├───Button
│           │       ButtonBase.cs
│           │       
│           ├───InputField
│           │       ValueInputFieldBase.cs
│           │       
│           ├───Slider
│           │       SliderBase.cs
│           │       
│           └───ValueUI
│                   SoundUIPart.cs
│                   ValueUIPart.cs
│                   
├───2_Interface (インターフェース)
│   │   
│   └───UI
│       │   
│       └───Panel
│               IPresenter.cs
│               
├───GameSetting (ゲーム全体の設定)
│       GameSeter.cs
│       
├───Manager (マネージャークラス)
│   │   
│   ├───Audio
│   │       AudioManager.cs
│   │       
│   ├───Panel
│   │       PausePanelManager.cs
│   │       SelectPanelManager.cs
│   │       
│   ├───Question
│   │       QuestionManager.cs
│   │       
│   ├───Save
│   │       SaveManager.cs
│   │       
│   ├───Scene
│   │       GameSceneManager.cs
│   │       
│   ├───Score
│   │       ScoreManager.cs
│   │       
│   ├───Stage
│   │       StageManager.cs
│   │       
│   ├───State
│   │       GameStateManager.cs
│   │       
│   └───Title
│           TitleManager.cs
│           
├───Object (オブジェクトを操作する)
│   │   
│   ├───Board
│   │       Board.cs
│   │       
│   ├───Frog
│   │       Frog.cs
│   │       
│   └───Swayng
│           SwayingGameObject.cs
│           
├───Operator (プレイヤーの操作)
│       PlayerOperator.cs
│       
└───UI
    │   
    ├───Group
    │   │   
    │   └───Question (お題パネルをまとめたUI)
    │           QuestionGroupPresenter.cs
    │           QuestionGroupView.cs
    │           
    ├───Panel
    │   │   
    │   ├───Finish (ゲーム終了時のUI)
    │   │       FinishPanel.cs
    │   │       
    │   ├───Pause (ポーズUI)
    │   │   │   
    │   │   ├───ConfirmPanel
    │   │   │       ConfirmPanelPresenter.cs
    │   │   │       ConfirmPanelView.cs
    │   │   │       
    │   │   ├───PauseMenu
    │   │   │       PauseMenuPanelPresenter.cs
    │   │   │       PauseMenuPanelView.cs
    │   │   │       
    │   │   └───SoundPanel
    │   │           PauseSoundPanelPresenter.cs
    │   │           SoundPanelView.cs
    │   │           
    │   ├───QuestionPanel (お題パネル)
    │   │       QuestionPanelPresenter.cs
    │   │       QuestionPanelView.cs
    │   │       
    │   ├───Select (メニューセレクトUI)
    │   │   │   
    │   │   ├───Credit
    │   │   │       CreditPanelPresenter.cs
    │   │   │       CreditPanelView.cs
    │   │   │       
    │   │   ├───HowToPlay
    │   │   │       HowToPlayPanelPresenter.cs
    │   │   │       HowToPlayPanelView.cs
    │   │   │       
    │   │   ├───Score
    │   │   │       ScorePanelPresenter.cs
    │   │   │       ScorePanelView.cs
    │   │   │       
    │   │   ├───Select
    │   │   │       SelectPanelPresenter.cs
    │   │   │       SelectPanelView.cs
    │   │   │       
    │   │   ├───Sound
    │   │   │       SelectSoundPanelPresenter.cs
    │   │   │       
    │   │   ├───StageSelect
    │   │   │       StageSelectPanelPresenter.cs
    │   │   │       StageSelectPanelView.cs
    │   │   │       
    │   │   └───Title
    │   │           TitlePanelPresenter.cs
    │   │           TitlePanelView.cs
    │   │           
    │   └───Slide
    │           SlidePanel.cs
    │           
    ├───Parts (UIパーツ)
    │   │   
    │   ├───Button
    │   │       ArrowButton.cs
    │   │       TextButton.cs
    │   │       ToggleButton.cs
    │   │       
    │   ├───Cell (お題のマス)
    │   │       QuestionPanelCellPresenter.cs
    │   │       QuestionPanelCellView.cs
    │   │       
    │   └───Timer
    │           TimerModel.cs
    │           TimerPresenter.cs
    │           TimerView.cs
    │           
    └───Result (結果UI)
        │   ResultUIPresenter.cs
        │   ResultUIView.cs
        │   
        └───HighScore
                HighScoreBubble.cs
```

## ゲームのURL
[UnityRoom](https://unityroom.com/games/kawarugawarukaeru)