using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PiroZangi
{
    // 効果音再生のためのインターフェースの作成
    public interface ISoundEffect
    {
        void SoundPlay(int c);
    }

    // メインクラス
    public partial class MainPage : ContentPage
    {
        private int remain;                 // 残り時間
        private int score;                  // スコア
        private int highscore;              // ハイスコア
        private int character;              // 出現キャラクタ
        private int interval;               // ザンギ出現間隔
        private int intervalcnt;            // 出現時間計数カウンタ
        private bool running;               // ゲームやってるよフラグ
        private int alive = 0;              // ザンギ出現穴番号
        private bool hit = false;           // 叩かれた？
        private ExImage[] _images;            // イメージコントロール配列
        
        // 乱数発生用変数
        System.Random r = new System.Random();

        // 効果音再生のためのインターフェースの実装
        ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        public MainPage()
        {
            int i;                          // 有象無象

            // おおもとの初期化
            InitializeComponent();
            running = false;

            // 開始の効果音
            using (soundEffect as IDisposable) {
                soundEffect.SoundPlay(6);
            }

            // 事前のハイスコア処理
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                             + "HighScore.txt";
            if (System.IO.File.Exists(localAppData)) {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(localAppData)) {
                    highscore = int.Parse(sr.ReadToEnd());
                }
            } else {
                highscore = 0;
            }
            highscoreLabel.Text = " HighScore: " + highscore.ToString("####0");

            // イメージ配列の格納
            Grid grid;
            grid = g;
            _images = new ExImage[9];
            for (i=0; i<9; i++) {
                _images[i] = new ExImage();
                _images[i].Source = ImageSource.FromResource("PiroZangi.image.plain.png");
                _images[i].BackgroundColor = Color.FromRgb(131, 225, 139);
                _images[i].TabIndex = i;
                grid.Children.Add(_images[i], i/3+1, i%3+1);
            }

            // タッチイベントの実装
            for(i=0; i<9; i++)
            {
                _images[i].Down += (sender, a) => {
                    if ((((ExImage)sender).TabIndex == alive) && (hit == false)) {
                        hit = true;
                        // 効果音
                        using (soundEffect as IDisposable) {
                            soundEffect.SoundPlay(character);
                        }
                        // キャラクタに応じてグラフィックを変更
                        switch (character) {
                            case 0:     // ザンギの場合
                                score += 100;
                                _images[alive].Source = ImageSource.FromResource("PiroZangi.image.zangi_p.png");
                                break;
                            case 1:     // 青のりダーの場合
                                score += 200;
                                _images[alive].Source = ImageSource.FromResource("PiroZangi.image.aonori_p.png");
                                break;
                            case 2:     // ピロ助の場合
                                score -= 50;
                                _images[alive].Source = ImageSource.FromResource("PiroZangi.image.piro_p.png");
                                break;
                            case 3:     // 店長の場合
                                score -= 1;
                                _images[alive].Source = ImageSource.FromResource("PiroZangi.image.ten_p.png");
                                break;
                            case 4:     // ゆいまーるの場合
                                score -= 10000;
                                _images[alive].Source = ImageSource.FromResource("PiroZangi.image.yui_p.png");
                                break;
                            case 5:     // キャプテンの場合
                                score = 0;
                                _images[alive].Source = ImageSource.FromResource("PiroZangi.image.cap_p.png");
                                break;
                            default:
                                break;
                        }

                        // スコアとハイスコア処理
                        scoreLabel.Text = "Score: " + score.ToString("####0");
                        if (score > highscore) {
                            highscore = score;
                            highscoreLabel.Text = "HighScore: " + highscore.ToString("####0");
                        }
                    }
                };
            }

            // タイマー処理
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () => {
                // 走ってなきゃ処理しない
                if (running == false) {
                    return true;
                }

                // 時間減算
                remain--;
                countBtn.Text = (remain / 100).ToString() + "." + (remain % 100).ToString("00");

                // 時間内なら
                if (remain > 0) {
                    intervalcnt--;
                    if (intervalcnt <= 0)
                    {
                        // 穴を戻して次の出現場所を選択
                        _images[alive].Source = ImageSource.FromResource("PiroZangi.image.plain.png");
                        hit = false;
                        alive = (alive + r.Next(1, 8)) % 9;

                        // キャラクタ選択
                        character = r.Next(0, 100);
                        if(character < 70) {
                            character = 0;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.zangi.png");
                        } else if (character < 80) {
                            character = 1;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.aonori.png");
                        } else if (character < 87) {
                            character = 2;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.piro.png");
                        } else if(character < 94) {
                            character = 3;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.ten.png");
                        } else if(character < 98) {
                            character = 4;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.yui.png");
                        } else {
                            character = 5;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.cap.png");
                        }

                        // 時間間隔調整
                        interval -= 3;
                        if (interval <= 10) interval = 10;
                        intervalcnt = interval;
                    }
                } else { // 時間をオーバーしたらそれで試合終了ですよ
                    // 終了の効果音
                    using (soundEffect as IDisposable) {
                        soundEffect.SoundPlay(6);
                    }
                    // ハイスコアの保存
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(localAppData)) {
                        sw.Write(highscore.ToString());
                    }
                    // 穴を元に戻してリプレイ確認
                    _images[alive].Source = ImageSource.FromResource("PiroZangi.image.plain.png");
                    countBtn.Text = "も う １ 回 ？";
                    running = false;
                    hit = false;
                }
                return true;
            });
        }

        // ゲーム開始ボタン
        void OnButtonClicked(object sender, EventArgs e)
        {
            // 走ってたら無視
            if (running == true) {
                return;
            }

            // 走ってなかったらもろもろの初期値を設定して走る
            for (int i = 0; i < 9; i++) {
                _images[i].Source = ImageSource.FromResource("PiroZangi.image.plain.png");
            }
            remain = 3000;
            score = 0;
            hit = false;
            interval = 132;
            intervalcnt = 0;
            alive = r.Next(0, 9);
            character = 0;
            scoreLabel.Text = "Score: " + score.ToString("####0");
            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.zangi.png");
            running = true;
        }
    }
}
