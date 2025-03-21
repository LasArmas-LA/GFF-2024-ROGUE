# GFF-2024-ROUGE

制作におけるチーム全員が効率よく作業ができるよう情報をまとめてあります。  
最新情報は随時追加予定  
ホームページにしてあるため知りたい情報があるときは見てみましょう。

アルファ版完成予定日は
# 12月31日 23時59分

## データ関連

### [Google ドライブ](https://drive.google.com/drive/folders/1qNsUP2GD4svIqNEIUb-bsCz-2AeszBqh?usp=drive_link)

### [仕様書-ExcelWeb](https://1drv.ms/x/c/1d234c969815360f/ET0u2ed3cJtCog1Kp05bh5wBwg3yCm4njS2yuXTas_RA5Q?e=oSuMdX)  

### [企画書](https://1drv.ms/p/c/1d234c969815360f/ESqPrr3c9TlEqiBNBiM6A5YBrcWuF49uA7VqEQeZ6kJBag?e=Xxi6W6)

### [イラストドライブ](https://drive.google.com/drive/folders/1UdTZgEOx7ecX6RMIFruUXqO9_YkQYcYf?usp=drive_link)

### [ドキュメント](https://drive.google.com/drive/folders/1k5T3OlEwNeazykKaJMlata8nsvBa4Ml3?usp=drive_link)

### [シナリオドライブ](https://drive.google.com/drive/folders/1lVZxuk0Klrc_xUi2M3J4TosaI_DBLwsS?usp=drive_link)

## 技術要素

| 開発環境     | バージョン       | 詳細                   |
| ------------ | ---------------- | ---------------------- |
| VisualStudio | 2024.17.12.0     | プログラムコードの入力 |
| Unity        | 6000.0.27f1(LTS) | ゲームエンジン         |
| Windows      | 11               | Windows 環境           |
| GitHub       |                  | データのリモート共有   |

## イラスト
下記はイラスト共有時に守ってもらいたいルールです。

- 提出は**グーグルドライブ**で提出してください。
（サイズが大きいときはギガファイルを使用しましょう）
- **PNG**データで共有をしてください。
- カラーモードは**RGB**で作成してください。
    - ラスタライズ効果は**PPI**に解像度は**350DPI(PPI)**としてください
- サイズ（画像比率）はプランナーに聞いてください。
- サイズ（幅と高さ）は**ピクセル**で作成してください。

## 命名規則
- 最初の文字は大文字にする
※英語、ローマ字記入の場合
- ファイル名は英語又はローマ字を使用すること。
- スペースは×
- アンダーバー( _ )を使用すること

## Git コミットメッセージ

GitHub の Commit（コミット）メッセージの意味は下記を参照

| 命名規則 | 意味                       |
| -------- | -------------------------- |
| fix      | バグ修正                   |
| add      | 既存ファイルや機能の追加   |
| update   | 機能修正                   |
| change   | 仕様による機能修正         |
| clean    | コードの改善、改善         |
| disable  | 機能を一時的に無効         |
| remove   | ファイルや、機能を削除     |
| rename   | ファイル名を変更           |
| move     | ファイルを移動             |
| upgrade  | バージョンアップ           |
| revert   | 以前のコミットに戻す<br>   |
| docs     | ドキュメントを修正<br>     |
| test     | テストコードの追加や修正   |
| chore    | その他                     |
| style    | コーディングスタイルの修正 |
| perf     | パフォーマンスを改善       |

### Live2Dで作成したキャラのレイヤー設定（Unity版）

- レイヤーバグの起きているPrefabを選択する

CubismRenderController
↓
Sorting
↓
Mode
↓
BackToFrontOrder 

の順で開き設定する
![PXL_20241203_095246362.jpg](https://prod-files-secure.s3.us-west-2.amazonaws.com/b2bf265f-e82f-4402-a513-a0617e88e031/a14f17c0-bf26-4076-be34-818620c2174b/PXL_20241203_095246362.jpg)


## 制作 Topics

- 質問をする  
  自己解決をするとチームとの認識合わせをしない事になる
  解らない単語・文章・概念は遠慮なく質問しよう！

- 仕様書をよく見よう  
  考えの食い違いがあると後々大変です。

- 概念を理解しよう  
  詳細よりも大枠を理解するだけでも全然違います。

- 解決よりも現状を整理しよう！
- Why?(なぜ？)を考えよう！
- 提出が間に合わないときは早めに相談を！！
- 意見があるときは共有しよう！

## 拡張機能

- Discord に GitHub 連携 BOT の実装  
  ┗ プッシュ時のメッセージを送信

### Created by LasArmas
