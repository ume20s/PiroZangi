using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PiroZangi
{
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
        private Image[] _images;            // イメージコントロール配列
        System.Random r = new System.Random();  // 乱数発生用変数

        public MainPage()
        {
            int i;                          // 有象無象

            InitializeComponent();
            running = false;

            // イメージ配列の格納
            _images = new Image[9];
            _images[0] = g0;
            _images[1] = g1;
            _images[2] = g2;
            _images[3] = g3;
            _images[4] = g4;
            _images[5] = g5;
            _images[6] = g6;
            _images[7] = g7;
            _images[8] = g8;

            // イメージ画像の初期化
            for(i=0; i<9; i++) {
                _images[i].Source = ImageSource.FromResource("PiroZangi.image.plain.png");
            }

            // タッチイベントの実装
            var gr = new TapGestureRecognizer();
            gr.Tapped += (sender, e) => {
                if((((Image)sender).TabIndex == alive) && (hit == false)) {
                    hit = true;
                    switch(character) {
                        case 0:     // ザンギの場合
                            score += 100;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.zangi_p.png");
                            break;
                        case 1:     // ピロ助の場合
                            score -= 50;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.piro_p.png");
                            break;
                        case 2:     // 店長の場合
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.ten_p.png");
                            break;
                        case 3:     // ゆいまーるの場合
                            score -= 10000;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.yui_p.png");
                            break;
                        case 4:     // キャプテンの場合
                            score = 0;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.cap_p.png");
                            break;
                        default:
                            break;
                    }
                }
                scoreLabel.Text = "Score: " + score.ToString("####0");
            };
            for(i=0; i<9; i++) {
                _images[i].GestureRecognizers.Add(gr);
            }


            // タイマー処理
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                // 走ってなきゃ処理しない
                if (running == false) {
                    return true;
                }

                // 時間減算
                remain--;
                countBtn.Text = (remain / 100).ToString() + "." + (remain % 100).ToString("00");

                // 時間内なら
                if (remain > 0)
                {
                    intervalcnt--;
                    if (intervalcnt <= 0)
                    {
                        // 穴を戻して次の出現場所を選択
                        _images[alive].Source = ImageSource.FromResource("PiroZangi.image.plain.png");
                        hit = false;
                        alive = (alive + r.Next(1, 8)) % 9;

                        // キャラクタ選択
                        character = r.Next(0, 100);
                        if(character < 82) {
                            character = 0;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.zangi.png");
                        } else if(character < 87) {
                            character = 1;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.piro.png");
                        } else if(character < 94) {
                            character = 2;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.ten.png");
                        } else if(character < 99) {
                            character = 3;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.yui.png");
                        } else {
                            character = 4;
                            _images[alive].Source = ImageSource.FromResource("PiroZangi.image.cap.png");
                        }

                        // 時間間隔調整
                        interval -= 3;
                        if (interval <= 10) interval = 10;
                        intervalcnt = interval;
                    }
                } else { // 時間をオーバーしたらそれで試合終了ですよ
                    _images[alive].Source = ImageSource.FromResource("PiroZangi.image.plain.png");
                    if(score > highscore)
                    {
                        highscore = score;
                        highscoreLabel.Text = "HighScore: " + highscore.ToString("####0");
                    }
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
