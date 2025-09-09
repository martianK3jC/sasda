using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalcApp.Core;
using CalcApp.Operations;
using System.Drawing.Drawing2D;
using System.Media;


namespace CalcApp
{
    public enum ThemeMode
    {
        Light,
        Dark,
        Ocean,
        Sunset,
        Forest,
        Neon,
        Heart  // Special mode, not part of regular cycling
    }

    public partial class CalcForm : Form
    {
        private System.Drawing.Text.PrivateFontCollection fontCollection;
        private FontFamily norwesterFont;

        double firstNumber = 0, secondNumber = 0;
        string operation = "";
        Dictionary<String, Func<Operation>> operations;
        bool isOpDone = false;
        bool justPressedOperator = false;
        bool justPressedEquals = false;
        string expressionChain = "";

        private ThemeMode currentTheme = ThemeMode.Ocean;

        private PictureBox heartGifBox;
        private Timer heartTimer;
        private Timer particleTimer;
        private List<HeartParticle> heartParticles;
        private Random random;
        private bool soundEnabled = false;
        

        public CalcForm()
        {
            // Enable double buffering to prevent flickering
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            
            LoadCustomFont();
            InitializeComponent();
            InitOperations();
            InitCalcAppDesign();
            InitButtonHoverEffects();
            InitParticleSystem();
            this.KeyPreview = true;
            this.KeyDown += CalcForm_KeyDown;
            this.Paint += CalcForm_Paint;
            heartBtn.Click += heartBtn_Click;
        }

        private void InitParticleSystem()
        {
            heartParticles = new List<HeartParticle>();
            random = new Random();
            
            particleTimer = new Timer();
            particleTimer.Interval = 50; // ~20 FPS - much smoother
            particleTimer.Tick += ParticleTimer_Tick;
        }

        private void ParticleTimer_Tick(object sender, EventArgs e)
        {
            if (currentTheme == ThemeMode.Heart)
            {
                // Add new particles randomly
                if (random.Next(0, 20) == 0) // 5% chance each frame - less frequent
                {
                    float x = (float)(random.NextDouble() * this.Width);
                    float y = this.Height + 20; // Start below the form
                    heartParticles.Add(new HeartParticle(x, y, random));
                }

                // Update existing particles
                for (int i = heartParticles.Count - 1; i >= 0; i--)
                {
                    heartParticles[i].Update();
                    if (!heartParticles[i].IsAlive || heartParticles[i].Y < -50)
                    {
                        heartParticles.RemoveAt(i);
                    }
                }

                // Only invalidate if there are active particles
                if (heartParticles.Count > 0)
                {
                    this.Invalidate();
                }
            }
        }

        private void PlayButtonSound()
        {
            if (soundEnabled)
            {
                try
                {
                    // Play system sound for button clicks
                    switch (currentTheme)
                    {
                        case ThemeMode.Heart:
                            SystemSounds.Exclamation.Play();
                            break;
                        case ThemeMode.Ocean:
                            SystemSounds.Asterisk.Play();
                            break;
                        case ThemeMode.Neon:
                            SystemSounds.Beep.Play();
                            break;
                        default:
                            SystemSounds.Hand.Play();
                            break;
                    }
                }
                catch
                {
                    // Silently ignore sound errors
                }
            }
        }

        private void InitButtonHoverEffects()
        {
            Button[] allButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn, addBtn, minusBtn, multiplyBtn, divideBtn,
                equalBtn, rmBtn, heartBtn, EraseWholeNumBtn, clearBtn,
                powerBtn, percentBtn, sqrtBtn, dlModeBtn
            };

            foreach (Button btn in allButtons)
            {
                btn.MouseEnter += Button_MouseEnter;
                btn.MouseLeave += Button_MouseLeave;
                btn.Tag = new { OriginalBackColor = btn.BackColor, OriginalForeColor = btn.ForeColor };
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Color hoverColor = GetHoverColor(btn.BackColor);
                btn.BackColor = hoverColor;
                
                // Add subtle animation effect
                btn.FlatAppearance.BorderSize = currentTheme == ThemeMode.Neon ? 2 : 1;
                
                // Special handling for heart button to maintain font
                if (btn == heartBtn)
                {
                    btn.Font = new Font(norwesterFont, 15, FontStyle.Regular);
                }
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                // Restore original colors without reapplying entire theme
                var originalColors = (dynamic)btn.Tag;
                btn.BackColor = originalColors.OriginalBackColor;
                btn.ForeColor = originalColors.OriginalForeColor;
                
                // Reset border size for non-neon themes
                if (currentTheme != ThemeMode.Neon)
                {
                    btn.FlatAppearance.BorderSize = 0;
                }
                else
                {
                    btn.FlatAppearance.BorderSize = 1;
                }
                
                // Special handling for heart button to maintain font
                if (btn == heartBtn)
                {
                    btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
                }
            }
        }

        private Color GetHoverColor(Color baseColor)
        {
            // Create a lighter/brighter version of the base color for hover effect
            int r = Math.Min(255, baseColor.R + 30);
            int g = Math.Min(255, baseColor.G + 30);
            int b = Math.Min(255, baseColor.B + 30);
            return Color.FromArgb(baseColor.A, r, g, b);
        }

        private void CalcForm_KeyDown1(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        //----------------------------designs----------------------------//
        private void InitCalcAppDesign()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "𓆩❤︎𓆪 Calculator";

            ApplyTheme(currentTheme);
        }
        


        // Light/Dark theme colors
        Color gray = ColorTranslator.FromHtml("#696969");
        Color darkGray = ColorTranslator.FromHtml("#555555");
        Color vividGreen = ColorTranslator.FromHtml("#5fc315");
        Color lightBlue = ColorTranslator.FromHtml("#d8f3f9");
        Color magenta = ColorTranslator.FromHtml("#da49a2");
        
        // Heart theme colors
        Color darkRed = ColorTranslator.FromHtml("#3a0621");
        Color hotPink = ColorTranslator.FromHtml("#f70060");
        Color lightPink = ColorTranslator.FromHtml("#ff4eb8");
        Color fuschia = ColorTranslator.FromHtml("#be005b");
        Color purple = ColorTranslator.FromHtml("#8a2765");

        // Ocean theme colors
        Color deepOcean = ColorTranslator.FromHtml("#0B2447");
        Color oceanBlue = ColorTranslator.FromHtml("#19376D");
        Color seaFoam = ColorTranslator.FromHtml("#576CBC");
        Color lightWave = ColorTranslator.FromHtml("#A5D7E8");
        Color pearlWhite = ColorTranslator.FromHtml("#F0F8FF");

        // Sunset theme colors
        Color sunsetDark = ColorTranslator.FromHtml("#2D1B69");
        Color sunsetPurple = ColorTranslator.FromHtml("#8E44AD");
        Color sunsetOrange = ColorTranslator.FromHtml("#E74C3C");
        Color sunsetYellow = ColorTranslator.FromHtml("#F39C12");
        Color sunsetLight = ColorTranslator.FromHtml("#FFF3E0");

        // Forest theme colors
        Color forestDark = ColorTranslator.FromHtml("#1B4332");
        Color forestGreen = ColorTranslator.FromHtml("#2D5016");
        Color leafGreen = ColorTranslator.FromHtml("#52B788");
        Color lightGreen = ColorTranslator.FromHtml("#95D5B2");
        Color mintCream = ColorTranslator.FromHtml("#F8FFF8");

        // Neon theme colors
        Color neonDark = ColorTranslator.FromHtml("#0A0A0A");
        Color neonPurple = ColorTranslator.FromHtml("#8A2BE2");
        Color neonCyan = ColorTranslator.FromHtml("#00FFFF");
        Color neonPink = ColorTranslator.FromHtml("#FF1493");
        Color neonWhite = ColorTranslator.FromHtml("#F0F0F0");


        private void ApplyTheme(ThemeMode theme)
        {
            currentTheme = theme;
            
            // Stop particle timer for non-heart themes
            if (theme != ThemeMode.Heart)
            {
                particleTimer?.Stop();
                heartParticles?.Clear();
            }
            switch (theme)
            {
                case ThemeMode.Light:
                    ApplyLightMode();
                    break;
                case ThemeMode.Dark:
                    ApplyDarkMode();
                    break;
                case ThemeMode.Heart:
                    ApplyHeartMode();
                    break;
                case ThemeMode.Ocean:
                    ApplyOceanMode();
                    break;
                case ThemeMode.Sunset:
                    ApplySunsetMode();
                    break;
                case ThemeMode.Forest:
                    ApplyForestMode();
                    break;
                case ThemeMode.Neon:
                    ApplyNeonMode();
                    break;
            }
            
            // Update button tags with new colors after theme change
            UpdateButtonTags();
        }

        private void UpdateButtonTags()
        {
            Button[] allButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn, addBtn, minusBtn, multiplyBtn, divideBtn,
                equalBtn, rmBtn, heartBtn, EraseWholeNumBtn, clearBtn,
                powerBtn, percentBtn, sqrtBtn, dlModeBtn
            };

            foreach (Button btn in allButtons)
            {
                btn.Tag = new { OriginalBackColor = btn.BackColor, OriginalForeColor = btn.ForeColor };
            }
        }

        private void ApplyLightMode()
        {
            this.BackColor = Color.White;

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = gray;
                btn.ForeColor = lightBlue;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            }
            
            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = magenta;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            // Special highlighting for heart button in Light theme
            heartBtn.BackColor = ColorTranslator.FromHtml("#ff69b4"); // Hot pink
            heartBtn.ForeColor = Color.White;
            heartBtn.FlatStyle = FlatStyle.Flat;
            heartBtn.FlatAppearance.BorderSize = 1;
            heartBtn.FlatAppearance.BorderColor = magenta;
            heartBtn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            
            Button[] operatorButtons2 =
            {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach(Button btn in operatorButtons2)
            {
                btn.BackColor = vividGreen;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes =
            {
                textDisplay, textExpression
            };

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = Color.White;
                tb.ForeColor = gray;
            }

            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            dlModeBtn.BackColor = lightBlue;
            dlModeBtn.ForeColor = darkGray;
            dlModeBtn.Text = ".‧₊˚ ☁︎ ☼ ༉‧₊˚.";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 22, FontStyle.Regular);


        }

        private void ApplyDarkMode()
        {
            this.BackColor = darkGray;

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = gray;
                btn.ForeColor = vividGreen;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = magenta;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            // Special highlighting for heart button in Dark theme
            heartBtn.BackColor = ColorTranslator.FromHtml("#ff1493"); // Deep pink
            heartBtn.ForeColor = Color.White;
            heartBtn.FlatStyle = FlatStyle.Flat;
            heartBtn.FlatAppearance.BorderSize = 1;
            heartBtn.FlatAppearance.BorderColor = vividGreen;
            heartBtn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            Button[] operatorButtons2 =
            {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = vividGreen;
                btn.ForeColor = darkGray;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes =
            {
                textDisplay, textExpression
            };

            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = Color.White;
                tb.ForeColor = gray;
            }

            dlModeBtn.BackColor = lightBlue;
            dlModeBtn.ForeColor = darkGray;
            dlModeBtn.Text = ".‧₊˚ ☁︎ ⏾ ༉‧₊˚.";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 22,FontStyle.Regular);
        }



        private void LoadCustomFont()
        {
            try
            {
                fontCollection = new System.Drawing.Text.PrivateFontCollection();
                string fontPath = Path.Combine(Application.StartupPath, "norwester.otf");
                if (File.Exists(fontPath))
                {
                    fontCollection.AddFontFile(fontPath);
                    norwesterFont = fontCollection.Families[0];
                }
                else
                {
                    norwesterFont = new FontFamily("Arial");
                }
            }
            catch
            {
                norwesterFont = new FontFamily("Arial");
            }
        }

    

        private void dlModeBtn_Click_1(object sender, EventArgs e)
        {
            // Cycle through themes (excluding Heart mode)
            switch (currentTheme)
            {
                case ThemeMode.Light:
                    ApplyTheme(ThemeMode.Dark);
                    break;
                case ThemeMode.Dark:
                    ApplyTheme(ThemeMode.Ocean);
                    break;
                case ThemeMode.Ocean:
                    ApplyTheme(ThemeMode.Sunset);
                    break;
                case ThemeMode.Sunset:
                    ApplyTheme(ThemeMode.Forest);
                    break;
                case ThemeMode.Forest:
                    ApplyTheme(ThemeMode.Neon);
                    break;
                case ThemeMode.Neon:
                    ApplyTheme(ThemeMode.Light);
                    break;
                case ThemeMode.Heart:
                    // If somehow in Heart mode, go to Dark mode
                    ApplyTheme(ThemeMode.Dark);
                    break;
            }
        }


        private void ApplyHeartMode()
        {
            this.Invalidate();
            particleTimer?.Start();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = hotPink;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = darkRed;
                btn.ForeColor = hotPink;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons2 =
            {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = fuschia;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes =
            {
                textDisplay, textExpression
            };

            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = Color.White;
                tb.ForeColor = hotPink;
            }

            dlModeBtn.BackColor = purple;
            dlModeBtn.ForeColor = Color.White;
            dlModeBtn.Text = "༺♡༻";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplyOceanMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = pearlWhite;
                btn.ForeColor = deepOcean;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = oceanBlue;
                btn.ForeColor = lightWave;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            // Special highlighting for heart button in Ocean theme
            heartBtn.BackColor = lightWave;
            heartBtn.ForeColor = deepOcean;
            heartBtn.FlatStyle = FlatStyle.Flat;
            heartBtn.FlatAppearance.BorderSize = 1;
            heartBtn.FlatAppearance.BorderColor = oceanBlue;
            heartBtn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = seaFoam;
                btn.ForeColor = pearlWhite;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = pearlWhite;
                tb.ForeColor = deepOcean;
            }

            dlModeBtn.BackColor = lightWave;
            dlModeBtn.ForeColor = deepOcean;
            dlModeBtn.Text = "🌊";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplySunsetMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = sunsetLight;
                btn.ForeColor = sunsetDark;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = sunsetPurple;
                btn.ForeColor = sunsetLight;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = sunsetOrange;
                btn.ForeColor = sunsetLight;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = sunsetLight;
                tb.ForeColor = sunsetDark;
            }

            dlModeBtn.BackColor = sunsetYellow;
            dlModeBtn.ForeColor = sunsetDark;
            dlModeBtn.Text = "🌅";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplyForestMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = mintCream;
                btn.ForeColor = forestDark;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = forestGreen;
                btn.ForeColor = mintCream;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = leafGreen;
                btn.ForeColor = mintCream;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = mintCream;
                tb.ForeColor = forestDark;
            }

            dlModeBtn.BackColor = lightGreen;
            dlModeBtn.ForeColor = forestDark;
            dlModeBtn.Text = "🌲";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplyNeonMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = neonDark;
                btn.ForeColor = neonCyan;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = neonCyan;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Bold);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = neonDark;
                btn.ForeColor = neonPink;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = neonPink;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Bold);
            }

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = neonDark;
                btn.ForeColor = neonPurple;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = neonPurple;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Bold);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Bold);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Bold);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = neonDark;
                tb.ForeColor = neonWhite;
            }

            dlModeBtn.BackColor = neonDark;
            dlModeBtn.ForeColor = neonCyan;
            dlModeBtn.Text = "⚡";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 1;
            dlModeBtn.FlatAppearance.BorderColor = neonCyan;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Bold);
        }

        private void CalcForm_Paint(object sender, PaintEventArgs e)
        {
            switch (currentTheme)
            {
                case ThemeMode.Heart:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { darkRed, fuschia, lightPink, hotPink, darkRed };
                        colorBlend.Positions = new float[] { 0.0f, 0.25f, 0.5f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    
                    // Draw heart particles
                    DrawHeartParticles(e.Graphics);
                    break;

                case ThemeMode.Ocean:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { deepOcean, oceanBlue, seaFoam, lightWave, pearlWhite };
                        colorBlend.Positions = new float[] { 0.0f, 0.3f, 0.6f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    break;

                case ThemeMode.Sunset:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { sunsetDark, sunsetPurple, sunsetOrange, sunsetYellow, sunsetLight };
                        colorBlend.Positions = new float[] { 0.0f, 0.2f, 0.5f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    break;

                case ThemeMode.Forest:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { forestDark, forestGreen, leafGreen, lightGreen, mintCream };
                        colorBlend.Positions = new float[] { 0.0f, 0.3f, 0.6f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    break;

                case ThemeMode.Neon:
                    using (PathGradientBrush pgb = new PathGradientBrush(new Point[] {
                        new Point(0, 0), new Point(this.Width, 0), 
                        new Point(this.Width, this.Height), new Point(0, this.Height) }))
                    {
                        pgb.CenterColor = Color.FromArgb(50, neonPurple);
                        pgb.SurroundColors = new Color[] { neonDark, neonDark, neonDark, neonDark };
                        e.Graphics.FillRectangle(pgb, this.ClientRectangle);
                    }
                    break;

                default:
                    // For Light and Dark modes, use solid colors (no gradient)
                    break;
            }
        }

        private void DrawHeartParticles(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            foreach (var particle in heartParticles)
            {
                if (particle.IsAlive)
                {
                    Color particleColor = Color.FromArgb(
                        (int)(particle.Alpha * 255), 
                        particle.Color);
                    
                    using (SolidBrush brush = new SolidBrush(particleColor))
                    {
                        // Draw heart shape (simplified as circle with heart symbol)
                        float size = particle.Size;
                        g.FillEllipse(brush, particle.X - size/2, particle.Y - size/2, size, size);
                        
                        // Draw heart symbol
                        using (Font font = new Font("Segoe UI Symbol", size * 0.6f))
                        {
                            string heartSymbol = "♥";
                            SizeF textSize = g.MeasureString(heartSymbol, font);
                            g.DrawString(heartSymbol, font, brush, 
                                particle.X - textSize.Width/2, 
                                particle.Y - textSize.Height/2);
                        }
                    }
                }
            }
        }

        private void ShowHeartExplosion()
        {
            // Stop any existing timer first
            if (heartTimer != null)
            {
                heartTimer.Stop();
                heartTimer.Dispose();
                heartTimer = null;
            }

            // Remove any existing gif box
            if (heartGifBox != null && this.Controls.Contains(heartGifBox))
            {
                this.Controls.Remove(heartGifBox);
                heartGifBox = null;
            }

            heartGifBox = new PictureBox();
            heartGifBox.Size = this.Size;
            heartGifBox.Location = new Point(0, 0);
            heartGifBox.BackColor = Color.Transparent;
            heartGifBox.SizeMode = PictureBoxSizeMode.StretchImage;

            string gifPath = Path.Combine(Application.StartupPath, "heart_explosion.gif");
            if (File.Exists(gifPath))
            {
                heartGifBox.Image = Image.FromFile(gifPath);
            }

            this.Controls.Add(heartGifBox);
            heartGifBox.BringToFront();

            heartTimer = new Timer();
            heartTimer.Interval = 3000;
            heartTimer.Tick += HeartTimer_Tick;
            heartTimer.Start();
        }
        private void HeartTimer_Tick(object sender, EventArgs e)
        {
            Timer currentTimer = heartTimer;

            if (currentTimer != null)
            {
                currentTimer.Tick -= HeartTimer_Tick;
                currentTimer.Stop();
                currentTimer.Dispose();
                heartTimer = null;
            }

            if (heartGifBox != null && this.Controls.Contains(heartGifBox))
            {
                heartGifBox.Visible = false;
                this.Controls.Remove(heartGifBox);
                heartGifBox = null;
            }

            textDisplay.Text = "0";
            textExpression.Clear();

            ApplyTheme(ThemeMode.Heart);
        }

        private void heartBtn_Click(object sender, EventArgs e)
        {
            if(textDisplay.Text == "143")
            {
                ShowHeartExplosion();
            }
            else
            {
                textExpression.ForeColor = hotPink;
                textExpression.TextAlign = HorizontalAlignment.Center;
                textExpression.Text = "Input 143, then click 𓆩❤︎𓆪";
                textExpression.TextAlign = HorizontalAlignment.Right;
            }
        }

        //----------------------------functions----------------------------//
        private void CalcForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                    SimulateNumberClick("0");
                    break;
                case Keys.D1:
                    SimulateNumberClick("1");
                    break;
                case Keys.D2:
                    SimulateNumberClick("2");
                    break;
                case Keys.D3:
                    SimulateNumberClick("3");
                    break;
                case Keys.D4:
                    SimulateNumberClick("4");
                    break;
                case Keys.D5:
                    SimulateNumberClick("5");
                    break;
                case Keys.D6:
                    SimulateNumberClick("6");
                    break;
                case Keys.D7:
                    SimulateNumberClick("7");
                    break;
                case Keys.D8:
                    if (e.Shift) // Shift + 8 gives *
                        SimulateOperatorClick("x");
                    else         // 8 key gives number 8
                        SimulateNumberClick("8");
                    break;
                case Keys.D9:
                    SimulateNumberClick("9");
                    break;
                case Keys.Oemplus:       // + key
                    if (e.Shift)         // Shift + = gives +
                        SimulateOperatorClick("+");
                    else                 // = key gives equals
                        SimulateEqualsClick();
                    break;
                case Keys.OemMinus:      // - key
                    SimulateOperatorClick("-");
                    break;
                case Keys.OemQuestion:   // / key
                    SimulateOperatorClick("÷");
                    break;

                // Decimal point
                case Keys.OemPeriod:     // . key
                    SimulateDecimalClick();
                    break;

                // Equals
                case Keys.Enter:
                    SimulateEqualsClick();
                    break;

                // Clear functions
                case Keys.Back:          // Backspace - remove one digit
                    rmBtn_Click(null, null);
                    break;
                case Keys.Escape:        // Escape - clear all
                    EraseWholeNumBtn_Click(null, null);
                    break;

                // Toggle sound
                case Keys.S:
                    if (e.Control)  // Ctrl+S toggles sound
                    {
                        soundEnabled = !soundEnabled;
                        // Visual feedback
                        string soundStatus = soundEnabled ? "ON" : "OFF";
                        textExpression.Text = $"Sound: {soundStatus}";
                        if (soundEnabled) PlayButtonSound();
                    }
                    break;
            }
        }

        private void SimulateNumberClick(string number)
        {
            Button mockButton = new Button() { Text = number };
            Number_Click(mockButton, null);
        }

        private void SimulateOperatorClick(string operatorText)
        {
            Button mockButton = new Button() { Text = operatorText };
            Operator_Click(mockButton, null);
        }

        private void SimulateDecimalClick()
        {
            Decimal_Click(null, null);
        }

        private void SimulateEqualsClick()
        {
            btnEquals_Click(null, null);
        }


        private void InitOperations()
        {
            operations = new Dictionary<string, Func<Operation>>()
            {
                { "+", () => new Addition() },
                { "-", () => new Subtraction() },
                { "x", () => new Multiplication() },
                { "÷", () => new Division() },
                { "^", () => new Power() },
                { "√", () => new SquareRoot() },
            };
        }



        void eraseDisplayIfOpDone()
        {
            if (isOpDone)
            {
                firstNumber = secondNumber = 0;
                operation = "";
                textDisplay.Clear();
                textExpression.Clear();
                isOpDone = false;
            }
        }

        //If textDisplay.Text is null or empty, return false
        private bool TryParseDisplay(out double result)
        {
            result = 0;
            if(string.IsNullOrEmpty(textDisplay.Text))
            {
                return false;
            }
            return double.TryParse(textDisplay.Text, out result);
        }

        private double PerformCalculation(double first, double second, string op)
        {
            if(!operations.ContainsKey(op))
            {
                throw new InvalidOperationException("Invalid operation");
            }

            if(op == "÷" && second == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }

            Operation operation = operations[op]();
            return operation.Execute(first, second);
        }

        void pressedEqualsFunc()
        {
            textExpression.Clear();
            textDisplay.Text = "";
            firstNumber = 0;
            operation = "";
            expressionChain = ""; 
            justPressedEquals = false;
        }

        private void Number_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            PlayButtonSound();

            if (justPressedOperator)
            {
                textDisplay.Text = "";
                justPressedOperator = false;
                isOpDone = false;
            }
            else if(justPressedEquals)
            {
                pressedEqualsFunc();
            } else if (isOpDone)
            {
                eraseDisplayIfOpDone();
            }

            //if (justPressedEquals && string.IsNullOrEmpty(operation))
            //{
            //    pressedEqualsFunc();
            //}
            //else if (justPressedEquals)
            //{
            //    justPressedEquals = false;
            //    isOpDone = false;
            //}
            //else
            //{
            //    eraseDisplayIfOpDone();
            //}

            if (textDisplay.Text == "0" && button.Text == "0")
            {
                return;
            }

            if(textDisplay.Text == "0" && button.Text != "0")
            {
                textDisplay.Text = button.Text;
            }
            else
            {
                textDisplay.Text += button.Text;
            }
        }

        private void Decimal_Click(object sender, EventArgs e)
        {
            if (justPressedOperator)
            {
                textDisplay.Text = "0.";
                justPressedOperator = false;
                return;
            }

            if(justPressedEquals)
            {
                textExpression.Clear();
                textDisplay.Text = "0.";
                firstNumber = 0;
                operation = "";
                justPressedEquals = false;
                isOpDone = false;
                return;
            }

            if(isOpDone) isOpDone = false;

            if (isOpDone && (string.IsNullOrEmpty(textDisplay.Text) || textDisplay.Text == "0"))
            {
                eraseDisplayIfOpDone();
            }

            if (string.IsNullOrEmpty(textDisplay.Text) || textDisplay.Text == "0")
            {
                textDisplay.Text = "0.";
            }
            else if (!textDisplay.Text.Contains("."))
            {
                textDisplay.Text += ".";
            }
        }

        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            PlayButtonSound();
            if (!TryParseDisplay(out double currentNumber))
            {
                currentNumber = 0;
                textDisplay.Text = "0";
            }
            
            if (justPressedEquals)
            {
                firstNumber = currentNumber;
                operation = button.Text;
                expressionChain = firstNumber + " ";
                textExpression.Text = expressionChain + button.Text;
                justPressedEquals = false;
                justPressedOperator = true;
                return;
            }
            
            if (!string.IsNullOrEmpty(operation) && !justPressedOperator)
            {
                try
                {
                    double result = PerformCalculation(firstNumber, currentNumber, operation);

                    if (double.IsNaN(result) || double.IsInfinity(result))
                    {
                        textDisplay.Text = "Error";
                        textExpression.Text = "Error";
                        isOpDone = true;
                        return;
                    }

                    if (string.IsNullOrEmpty(expressionChain))
                        expressionChain = firstNumber + " " + operation + " " + currentNumber + " ";
                    else
                        expressionChain += operation + " " + currentNumber + " ";

                    firstNumber = result;
                    textDisplay.Text = result.ToString();

                    textExpression.Text = expressionChain + button.Text;
                }
                catch (Exception ex)
                {
                    textDisplay.Text = "Error";
                    textExpression.Text = "Error";
                    isOpDone = true;
                    return;
                }
            }
            else
            {
                firstNumber = currentNumber;
                expressionChain = firstNumber + " ";
                textExpression.Text = expressionChain + button.Text;
            }

            operation = button.Text;
            justPressedOperator = true;
        }

        private void btnEquals_Click(object sender, EventArgs e) { 
            PlayButtonSound();
            //secondNumber = if textDisplay.Text not empty then Convert.ToDouble(textDisplay.Text) else 0
            if(!TryParseDisplay(out double secondNumber)) //If it's true that it can parse, then assign the parsed value to secondNumber
            {
                secondNumber = 0;
            }

            if (justPressedEquals)
            {
                // Repeat the last operation
                if (!string.IsNullOrEmpty(operation))
                {
                    if (!TryParseDisplay(out double currentResult))
                        return;

                    try
                    {
                        double result = PerformCalculation(currentResult, this.secondNumber, operation);
                        textDisplay.Text = result.ToString();
                        textExpression.Text = currentResult + " " + operation + " " + this.secondNumber + " = " ;
                    }
                    catch (Exception ex)
                    {
                        textDisplay.Text = "Error";
                        textExpression.Text = "Error";
                    }
                }
                return;
            }

            if (operations.ContainsKey(operation))
            {
                if (operation == "÷" && secondNumber == 0)
                {
                    textDisplay.Text = "Cannot divide by 0";
                    textExpression.Text = firstNumber + " " + operation + " " + secondNumber + " = ";
                    isOpDone = true;
                    return;
                }
                try
                {
                    Operation op = operations[operation](); // Create instance of the Operation, operations(is the dictionary) at key operation (like +, -, etc)
                    double result = op.Execute(firstNumber, secondNumber);

                    if (double.IsNaN(result) || double.IsInfinity(result))
                    {
                        textDisplay.Text = "Error";
                        textExpression.Text = firstNumber + " " + operation + " " + secondNumber + " = ";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(expressionChain))
                        {
                            textExpression.Text = firstNumber + " " + operation + " " + secondNumber + " = ";
                            textDisplay.Text = result.ToString();
                        }
                        else
                        {
                            textDisplay.Text = result.ToString();
                            textExpression.Text = expressionChain + operation + " " + secondNumber + " = " ;
                        }
                    }
                    this.secondNumber = secondNumber;
                    justPressedEquals = true;
                    isOpDone = true;
                }
                catch(Exception ex)
                {
                    textDisplay.Text = "Error";
                    textExpression.Text = "Error";
                    isOpDone = true;
                }
            }
        }


        private void rmBtn_Click(object sender, EventArgs e)
        {
            justPressedOperator = false;
            justPressedEquals = false;

            if (textDisplay.Text.Length > 0)
            {
                textDisplay.Text = textDisplay.Text.Remove(textDisplay.Text.Length - 1);
                if(string.IsNullOrEmpty(textDisplay.Text))
                {
                    textDisplay.Text = "0";
                }
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            textDisplay.Clear();
            secondNumber = 0;
        }

        private void signBtn_Click(object sender, EventArgs e)
        {
            if(TryParseDisplay(out double currentNumber))
            {
                if(currentNumber == 0)
                {
                    return;
                }

                currentNumber = -currentNumber;
                textDisplay.Text = currentNumber.ToString();
                justPressedOperator = false;
                justPressedEquals = false;
            }
        }

        private void EraseWholeNumBtn_Click(object sender, EventArgs e)
        {
            textDisplay.Text = "0";
            textExpression.Clear();
            firstNumber = secondNumber = 0;
            operation = "";
            expressionChain = "";
            isOpDone = false;
            justPressedOperator = false;
            justPressedEquals = false;
        }

        private void Percent_Click(object sender, EventArgs e)
        {
            if(TryParseDisplay(out double num))
            {
                double result = num/ 100.0;
                textDisplay.Text = result.ToString();
                textExpression.Text = num + " % = ";
                isOpDone = true;
                justPressedEquals = false;
                justPressedOperator = false;
            }
            else
            {
                textDisplay.Text = "0";
            }
        }

        private void CalcForm_Load(object sender, EventArgs e)
        {

        }

        

        private void btnSqrt_Click(object sender, EventArgs e)
        {
            if (!TryParseDisplay(out double num))
            {
                textDisplay.Text = "Error";
                textExpression.Text = "Error";
                isOpDone = true;
                return;
            }

            if (num < 0)
            {
                textDisplay.Text = "Cannot calculate square root of negative number";
                textExpression.Text = "√" + num + " = Error";
                isOpDone = true;
                return;
            }

            try
            {
                Operation op = operations["√"]();
                double result = op.Execute(num, 0);
                textDisplay.Text = result.ToString();
                textExpression.Text = "√" + num + " = ";
                isOpDone = true;
                justPressedEquals = false;
                justPressedOperator = false;
            }
            catch (Exception ex)
            {
                textDisplay.Text = "Error";
                textExpression.Text = "Error";
                isOpDone = true;
            }
        }
    }

    public class HeartParticle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Size { get; set; }
        public Color Color { get; set; }
        public float Alpha { get; set; }
        public int Life { get; set; }
        public int MaxLife { get; set; }

        public HeartParticle(float x, float y, Random rand)
        {
            X = x;
            Y = y;
            VelocityX = (float)(rand.NextDouble() * 4 - 2);
            VelocityY = (float)(rand.NextDouble() * 2 - 4);
            Size = (float)(rand.NextDouble() * 15 + 10);
            Alpha = 1.0f;
            Life = 0;
            MaxLife = rand.Next(60, 120);
            
            Color[] heartColors = { Color.HotPink, Color.DeepPink, Color.Pink, Color.LightPink };
            Color = heartColors[rand.Next(heartColors.Length)];
        }

        public void Update()
        {
            X += VelocityX;
            Y += VelocityY;
            VelocityY += 0.1f; // Gravity effect
            Life++;
            Alpha = 1.0f - (float)Life / MaxLife;
        }

        public bool IsAlive => Life < MaxLife && Alpha > 0;
    }

    public enum ThemeMode
    {
        Light,
        Dark,
        Ocean,
        Sunset,
        Forest,
        Neon,
        Heart  // Special mode, not part of regular cycling
    }

    public partial class CalcForm : Form
    {
        private System.Drawing.Text.PrivateFontCollection fontCollection;
        private FontFamily norwesterFont;

        double firstNumber = 0, secondNumber = 0;
        string operation = "";
        Dictionary<String, Func<Operation>> operations;
        bool isOpDone = false;
        bool justPressedOperator = false;
        bool justPressedEquals = false;
        string expressionChain = "";

        private ThemeMode currentTheme = ThemeMode.Ocean;

        private PictureBox heartGifBox;
        private Timer heartTimer;
        private Timer particleTimer;
        private List<HeartParticle> heartParticles;
        private Random random;
        private bool soundEnabled = false;
        

        public CalcForm()
        {
            // Enable double buffering to prevent flickering
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            
            LoadCustomFont();
            InitializeComponent();
            InitOperations();
            InitCalcAppDesign();
            InitButtonHoverEffects();
            InitParticleSystem();
            this.KeyPreview = true;
            this.KeyDown += CalcForm_KeyDown;
            this.Paint += CalcForm_Paint;
            heartBtn.Click += heartBtn_Click;
        }

        private void InitParticleSystem()
        {
            heartParticles = new List<HeartParticle>();
            random = new Random();
            
            particleTimer = new Timer();
            particleTimer.Interval = 50; // ~20 FPS - much smoother
            particleTimer.Tick += ParticleTimer_Tick;
        }

        private void ParticleTimer_Tick(object sender, EventArgs e)
        {
            if (currentTheme == ThemeMode.Heart)
            {
                // Add new particles randomly
                if (random.Next(0, 20) == 0) // 5% chance each frame - less frequent
                {
                    float x = (float)(random.NextDouble() * this.Width);
                    float y = this.Height + 20; // Start below the form
                    heartParticles.Add(new HeartParticle(x, y, random));
                }

                // Update existing particles
                for (int i = heartParticles.Count - 1; i >= 0; i--)
                {
                    heartParticles[i].Update();
                    if (!heartParticles[i].IsAlive || heartParticles[i].Y < -50)
                    {
                        heartParticles.RemoveAt(i);
                    }
                }

                // Only invalidate if there are active particles
                if (heartParticles.Count > 0)
                {
                    this.Invalidate();
                }
            }
        }

        private void PlayButtonSound()
        {
            if (soundEnabled)
            {
                try
                {
                    // Play system sound for button clicks
                    switch (currentTheme)
                    {
                        case ThemeMode.Heart:
                            SystemSounds.Exclamation.Play();
                            break;
                        case ThemeMode.Ocean:
                            SystemSounds.Asterisk.Play();
                            break;
                        case ThemeMode.Neon:
                            SystemSounds.Beep.Play();
                            break;
                        default:
                            SystemSounds.Hand.Play();
                            break;
                    }
                }
                catch
                {
                    // Silently ignore sound errors
                }
            }
        }

        private void InitButtonHoverEffects()
        {
            Button[] allButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn, addBtn, minusBtn, multiplyBtn, divideBtn,
                equalBtn, rmBtn, heartBtn, EraseWholeNumBtn, clearBtn,
                powerBtn, percentBtn, sqrtBtn, dlModeBtn
            };

            foreach (Button btn in allButtons)
            {
                btn.MouseEnter += Button_MouseEnter;
                btn.MouseLeave += Button_MouseLeave;
                btn.Tag = new { OriginalBackColor = btn.BackColor, OriginalForeColor = btn.ForeColor };
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Color hoverColor = GetHoverColor(btn.BackColor);
                btn.BackColor = hoverColor;
                
                // Add subtle animation effect
                btn.FlatAppearance.BorderSize = currentTheme == ThemeMode.Neon ? 2 : 1;
                
                // Special handling for heart button to maintain font
                if (btn == heartBtn)
                {
                    btn.Font = new Font(norwesterFont, 15, FontStyle.Regular);
                }
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                // Restore original colors without reapplying entire theme
                var originalColors = (dynamic)btn.Tag;
                btn.BackColor = originalColors.OriginalBackColor;
                btn.ForeColor = originalColors.OriginalForeColor;
                
                // Reset border size for non-neon themes
                if (currentTheme != ThemeMode.Neon)
                {
                    btn.FlatAppearance.BorderSize = 0;
                }
                else
                {
                    btn.FlatAppearance.BorderSize = 1;
                }
                
                // Special handling for heart button to maintain font
                if (btn == heartBtn)
                {
                    btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
                }
            }
        }

        private Color GetHoverColor(Color baseColor)
        {
            // Create a lighter/brighter version of the base color for hover effect
            int r = Math.Min(255, baseColor.R + 30);
            int g = Math.Min(255, baseColor.G + 30);
            int b = Math.Min(255, baseColor.B + 30);
            return Color.FromArgb(baseColor.A, r, g, b);
        }

        private void CalcForm_KeyDown1(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        //----------------------------designs----------------------------//
        private void InitCalcAppDesign()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "𓆩❤︎𓆪 Calculator";

            ApplyTheme(currentTheme);
        }
        



        // Light/Dark theme colors
        Color gray = ColorTranslator.FromHtml("#696969");
        Color darkGray = ColorTranslator.FromHtml("#555555");
        Color vividGreen = ColorTranslator.FromHtml("#5fc315");
        Color lightBlue = ColorTranslator.FromHtml("#d8f3f9");
        Color magenta = ColorTranslator.FromHtml("#da49a2");
        
        // Heart theme colors
        Color darkRed = ColorTranslator.FromHtml("#3a0621");
        Color hotPink = ColorTranslator.FromHtml("#f70060");
        Color lightPink = ColorTranslator.FromHtml("#ff4eb8");
        Color fuschia = ColorTranslator.FromHtml("#be005b");
        Color purple = ColorTranslator.FromHtml("#8a2765");

        // Ocean theme colors
        Color deepOcean = ColorTranslator.FromHtml("#0B2447");
        Color oceanBlue = ColorTranslator.FromHtml("#19376D");
        Color seaFoam = ColorTranslator.FromHtml("#576CBC");
        Color lightWave = ColorTranslator.FromHtml("#A5D7E8");
        Color pearlWhite = ColorTranslator.FromHtml("#F0F8FF");

        // Sunset theme colors
        Color sunsetDark = ColorTranslator.FromHtml("#2D1B69");
        Color sunsetPurple = ColorTranslator.FromHtml("#8E44AD");
        Color sunsetOrange = ColorTranslator.FromHtml("#E74C3C");
        Color sunsetYellow = ColorTranslator.FromHtml("#F39C12");
        Color sunsetLight = ColorTranslator.FromHtml("#FFF3E0");

        // Forest theme colors
        Color forestDark = ColorTranslator.FromHtml("#1B4332");
        Color forestGreen = ColorTranslator.FromHtml("#2D5016");
        Color leafGreen = ColorTranslator.FromHtml("#52B788");
        Color lightGreen = ColorTranslator.FromHtml("#95D5B2");
        Color mintCream = ColorTranslator.FromHtml("#F8FFF8");

        // Neon theme colors
        Color neonDark = ColorTranslator.FromHtml("#0A0A0A");
        Color neonPurple = ColorTranslator.FromHtml("#8A2BE2");
        Color neonCyan = ColorTranslator.FromHtml("#00FFFF");
        Color neonPink = ColorTranslator.FromHtml("#FF1493");
        Color neonWhite = ColorTranslator.FromHtml("#F0F0F0");


        private void ApplyTheme(ThemeMode theme)
        {
            currentTheme = theme;
            
            // Stop particle timer for non-heart themes
            if (theme != ThemeMode.Heart)
            {
                particleTimer?.Stop();
                heartParticles?.Clear();
            }
            switch (theme)
            {
                case ThemeMode.Light:
                    ApplyLightMode();
                    break;
                case ThemeMode.Dark:
                    ApplyDarkMode();
                    break;
                case ThemeMode.Heart:
                    ApplyHeartMode();
                    break;
                case ThemeMode.Ocean:
                    ApplyOceanMode();
                    break;
                case ThemeMode.Sunset:
                    ApplySunsetMode();
                    break;
                case ThemeMode.Forest:
                    ApplyForestMode();
                    break;
                case ThemeMode.Neon:
                    ApplyNeonMode();
                    break;
            }
            
            // Update button tags with new colors after theme change
            UpdateButtonTags();
        }

        private void UpdateButtonTags()
        {
            Button[] allButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn, addBtn, minusBtn, multiplyBtn, divideBtn,
                equalBtn, rmBtn, heartBtn, EraseWholeNumBtn, clearBtn,
                powerBtn, percentBtn, sqrtBtn, dlModeBtn
            };

            foreach (Button btn in allButtons)
            {
                btn.Tag = new { OriginalBackColor = btn.BackColor, OriginalForeColor = btn.ForeColor };
            }
        }

        private void ApplyLightMode()
        {
            this.BackColor = Color.White;

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = gray;
                btn.ForeColor = lightBlue;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            }
            
            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = magenta;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            // Special highlighting for heart button in Light theme
            heartBtn.BackColor = ColorTranslator.FromHtml("#ff69b4"); // Hot pink
            heartBtn.ForeColor = Color.White;
            heartBtn.FlatStyle = FlatStyle.Flat;
            heartBtn.FlatAppearance.BorderSize = 1;
            heartBtn.FlatAppearance.BorderColor = magenta;
            heartBtn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            
            Button[] operatorButtons2 =
            {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach(Button btn in operatorButtons2)
            {
                btn.BackColor = vividGreen;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes =
            {
                textDisplay, textExpression
            };

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = Color.White;
                tb.ForeColor = gray;
            }

            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            dlModeBtn.BackColor = lightBlue;
            dlModeBtn.ForeColor = darkGray;
            dlModeBtn.Text = ".‧₊˚ ☁︎ ☼ ༉‧₊˚.";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 22, FontStyle.Regular);


        }

        private void ApplyDarkMode()
        {
            this.BackColor = darkGray;

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = gray;
                btn.ForeColor = vividGreen;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = magenta;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            // Special highlighting for heart button in Dark theme
            heartBtn.BackColor = ColorTranslator.FromHtml("#ff1493"); // Deep pink
            heartBtn.ForeColor = Color.White;
            heartBtn.FlatStyle = FlatStyle.Flat;
            heartBtn.FlatAppearance.BorderSize = 1;
            heartBtn.FlatAppearance.BorderColor = vividGreen;
            heartBtn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            Button[] operatorButtons2 =
            {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = vividGreen;
                btn.ForeColor = darkGray;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes =
            {
                textDisplay, textExpression
            };

            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = Color.White;
                tb.ForeColor = gray;
            }

            dlModeBtn.BackColor = lightBlue;
            dlModeBtn.ForeColor = darkGray;
            dlModeBtn.Text = ".‧₊˚ ☁︎ ⏾ ༉‧₊˚.";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 22,FontStyle.Regular);
        }



        private void LoadCustomFont()
        {
            try
            {
                fontCollection = new System.Drawing.Text.PrivateFontCollection();
                string fontPath = Path.Combine(Application.StartupPath, "norwester.otf");
                if (File.Exists(fontPath))
                {
                    fontCollection.AddFontFile(fontPath);
                    norwesterFont = fontCollection.Families[0];
                }
                else
                {
                    norwesterFont = new FontFamily("Arial");
                }
            }
            catch
            {
                norwesterFont = new FontFamily("Arial");
            }
        }

    

        private void dlModeBtn_Click_1(object sender, EventArgs e)
        {
            // Cycle through themes (excluding Heart mode)
            switch (currentTheme)
            {
                case ThemeMode.Light:
                    ApplyTheme(ThemeMode.Dark);
                    break;
                case ThemeMode.Dark:
                    ApplyTheme(ThemeMode.Ocean);
                    break;
                case ThemeMode.Ocean:
                    ApplyTheme(ThemeMode.Sunset);
                    break;
                case ThemeMode.Sunset:
                    ApplyTheme(ThemeMode.Forest);
                    break;
                case ThemeMode.Forest:
                    ApplyTheme(ThemeMode.Neon);
                    break;
                case ThemeMode.Neon:
                    ApplyTheme(ThemeMode.Light);
                    break;
                case ThemeMode.Heart:
                    // If somehow in Heart mode, go to Dark mode
                    ApplyTheme(ThemeMode.Dark);
                    break;
            }
        }


        private void ApplyHeartMode()
        {
            this.Invalidate();
            particleTimer?.Start();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = hotPink;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = darkRed;
                btn.ForeColor = hotPink;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons2 =
            {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = fuschia;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes =
            {
                textDisplay, textExpression
            };

            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = Color.White;
                tb.ForeColor = hotPink;
            }

            dlModeBtn.BackColor = purple;
            dlModeBtn.ForeColor = Color.White;
            dlModeBtn.Text = "༺♡༻";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplyOceanMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = pearlWhite;
                btn.ForeColor = deepOcean;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = oceanBlue;
                btn.ForeColor = lightWave;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            // Special highlighting for heart button in Ocean theme
            heartBtn.BackColor = lightWave;
            heartBtn.ForeColor = deepOcean;
            heartBtn.FlatStyle = FlatStyle.Flat;
            heartBtn.FlatAppearance.BorderSize = 1;
            heartBtn.FlatAppearance.BorderColor = oceanBlue;
            heartBtn.Font = new Font(norwesterFont, 16, FontStyle.Regular);

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = seaFoam;
                btn.ForeColor = pearlWhite;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = pearlWhite;
                tb.ForeColor = deepOcean;
            }

            dlModeBtn.BackColor = lightWave;
            dlModeBtn.ForeColor = deepOcean;
            dlModeBtn.Text = "🌊";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplySunsetMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = sunsetLight;
                btn.ForeColor = sunsetDark;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = sunsetPurple;
                btn.ForeColor = sunsetLight;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = sunsetOrange;
                btn.ForeColor = sunsetLight;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = sunsetLight;
                tb.ForeColor = sunsetDark;
            }

            dlModeBtn.BackColor = sunsetYellow;
            dlModeBtn.ForeColor = sunsetDark;
            dlModeBtn.Text = "🌅";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplyForestMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = mintCream;
                btn.ForeColor = forestDark;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = forestGreen;
                btn.ForeColor = mintCream;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = leafGreen;
                btn.ForeColor = mintCream;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Regular);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Regular);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Regular);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = mintCream;
                tb.ForeColor = forestDark;
            }

            dlModeBtn.BackColor = lightGreen;
            dlModeBtn.ForeColor = forestDark;
            dlModeBtn.Text = "🌲";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 0;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Regular);
        }

        private void ApplyNeonMode()
        {
            this.Invalidate();

            Button[] numberButtons = {
                btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
                decimalBtn, signBtn
            };

            foreach (Button btn in numberButtons)
            {
                btn.BackColor = neonDark;
                btn.ForeColor = neonCyan;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = neonCyan;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Bold);
            }

            Button[] operatorButtons1 = {
                addBtn, minusBtn, multiplyBtn, equalBtn, rmBtn, heartBtn
            };

            foreach (Button btn in operatorButtons1)
            {
                btn.BackColor = neonDark;
                btn.ForeColor = neonPink;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = neonPink;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Bold);
            }

            Button[] operatorButtons2 = {
                EraseWholeNumBtn, clearBtn, powerBtn, divideBtn, percentBtn, sqrtBtn
            };

            foreach (Button btn in operatorButtons2)
            {
                btn.BackColor = neonDark;
                btn.ForeColor = neonPurple;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = neonPurple;
                btn.Font = new Font(norwesterFont, 16, FontStyle.Bold);
            }

            TextBox[] textboxes = { textDisplay, textExpression };
            textDisplay.Font = new Font(norwesterFont, 45, FontStyle.Bold);
            textExpression.Font = new Font(norwesterFont, 15, FontStyle.Bold);

            foreach (TextBox tb in textboxes)
            {
                tb.BackColor = neonDark;
                tb.ForeColor = neonWhite;
            }

            dlModeBtn.BackColor = neonDark;
            dlModeBtn.ForeColor = neonCyan;
            dlModeBtn.Text = "⚡";
            dlModeBtn.FlatStyle = FlatStyle.Flat;
            dlModeBtn.FlatAppearance.BorderSize = 1;
            dlModeBtn.FlatAppearance.BorderColor = neonCyan;
            dlModeBtn.Font = new Font(norwesterFont, 25, FontStyle.Bold);
        }

        private void CalcForm_Paint(object sender, PaintEventArgs e)
        {
            switch (currentTheme)
            {
                case ThemeMode.Heart:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { darkRed, fuschia, lightPink, hotPink, darkRed };
                        colorBlend.Positions = new float[] { 0.0f, 0.25f, 0.5f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    
                    // Draw heart particles
                    DrawHeartParticles(e.Graphics);
                    break;

                case ThemeMode.Ocean:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { deepOcean, oceanBlue, seaFoam, lightWave, pearlWhite };
                        colorBlend.Positions = new float[] { 0.0f, 0.3f, 0.6f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    break;

                case ThemeMode.Sunset:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { sunsetDark, sunsetPurple, sunsetOrange, sunsetYellow, sunsetLight };
                        colorBlend.Positions = new float[] { 0.0f, 0.2f, 0.5f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    break;

                case ThemeMode.Forest:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        colorBlend.Colors = new Color[] { forestDark, forestGreen, leafGreen, lightGreen, mintCream };
                        colorBlend.Positions = new float[] { 0.0f, 0.3f, 0.6f, 0.8f, 1.0f };
                        brush.InterpolationColors = colorBlend;
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                    break;

                case ThemeMode.Neon:
                    using (PathGradientBrush pgb = new PathGradientBrush(new Point[] {
                        new Point(0, 0), new Point(this.Width, 0), 
                        new Point(this.Width, this.Height), new Point(0, this.Height) }))
                    {
                        pgb.CenterColor = Color.FromArgb(50, neonPurple);
                        pgb.SurroundColors = new Color[] { neonDark, neonDark, neonDark, neonDark };
                        e.Graphics.FillRectangle(pgb, this.ClientRectangle);
                    }
                    break;

                default:
                    // For Light and Dark modes, use solid colors (no gradient)
                    break;
            }
        }

        private void DrawHeartParticles(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            foreach (var particle in heartParticles)
            {
                if (particle.IsAlive)
                {
                    Color particleColor = Color.FromArgb(
                        (int)(particle.Alpha * 255), 
                        particle.Color);
                    
                    using (SolidBrush brush = new SolidBrush(particleColor))
                    {
                        // Draw heart shape (simplified as circle with heart symbol)
                        float size = particle.Size;
                        g.FillEllipse(brush, particle.X - size/2, particle.Y - size/2, size, size);
                        
                        // Draw heart symbol
                        using (Font font = new Font("Segoe UI Symbol", size * 0.6f))
                        {
                            string heartSymbol = "♥";
                            SizeF textSize = g.MeasureString(heartSymbol, font);
                            g.DrawString(heartSymbol, font, brush, 
                                particle.X - textSize.Width/2, 
                                particle.Y - textSize.Height/2);
                        }
                    }
                }
            }
        }

        private void ShowHeartExplosion()
        {
            // Stop any existing timer first
            if (heartTimer != null)
            {
                heartTimer.Stop();
                heartTimer.Dispose();
                heartTimer = null;
            }

            // Remove any existing gif box
            if (heartGifBox != null && this.Controls.Contains(heartGifBox))
            {
                this.Controls.Remove(heartGifBox);
                heartGifBox = null;
            }

            heartGifBox = new PictureBox();
            heartGifBox.Size = this.Size;
            heartGifBox.Location = new Point(0, 0);
            heartGifBox.BackColor = Color.Transparent;
            heartGifBox.SizeMode = PictureBoxSizeMode.StretchImage;

            string gifPath = Path.Combine(Application.StartupPath, "heart_explosion.gif");
            if (File.Exists(gifPath))
            {
                heartGifBox.Image = Image.FromFile(gifPath);
            }

            this.Controls.Add(heartGifBox);
            heartGifBox.BringToFront();

            heartTimer = new Timer();
            heartTimer.Interval = 3000;
            heartTimer.Tick += HeartTimer_Tick;
            heartTimer.Start();
        }
        private void HeartTimer_Tick(object sender, EventArgs e)
        {
            Timer currentTimer = heartTimer;

            if (currentTimer != null)
            {
                currentTimer.Tick -= HeartTimer_Tick;
                currentTimer.Stop();
                currentTimer.Dispose();
                heartTimer = null;
            }

            if (heartGifBox != null && this.Controls.Contains(heartGifBox))
            {
                heartGifBox.Visible = false;
                this.Controls.Remove(heartGifBox);
                heartGifBox = null;
            }

            textDisplay.Text = "0";
            textExpression.Clear();

            ApplyTheme(ThemeMode.Heart);
        }

        private void heartBtn_Click(object sender, EventArgs e)
        {
            if(textDisplay.Text == "143")
            {
                ShowHeartExplosion();
            }
            else
            {
                textExpression.ForeColor = hotPink;
                textExpression.TextAlign = HorizontalAlignment.Center;
                textExpression.Text = "Input 143, then click 𓆩❤︎𓆪";
                textExpression.TextAlign = HorizontalAlignment.Right;
            }
        }

        //----------------------------functions----------------------------//
        private void CalcForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                    SimulateNumberClick("0");
                    break;
                case Keys.D1:
                    SimulateNumberClick("1");
                    break;
                case Keys.D2:
                    SimulateNumberClick("2");
                    break;
                case Keys.D3:
                    SimulateNumberClick("3");
                    break;
                case Keys.D4:
                    SimulateNumberClick("4");
                    break;
                case Keys.D5:
                    SimulateNumberClick("5");
                    break;
                case Keys.D6:
                    SimulateNumberClick("6");
                    break;
                case Keys.D7:
                    SimulateNumberClick("7");
                    break;
                case Keys.D8:
                    if (e.Shift) // Shift + 8 gives *
                        SimulateOperatorClick("x");
                    else         // 8 key gives number 8
                        SimulateNumberClick("8");
                    break;
                case Keys.D9:
                    SimulateNumberClick("9");
                    break;
                case Keys.Oemplus:       // + key
                    if (e.Shift)         // Shift + = gives +
                        SimulateOperatorClick("+");
                    else                 // = key gives equals
                        SimulateEqualsClick();
                    break;
                case Keys.OemMinus:      // - key
                    SimulateOperatorClick("-");
                    break;
                case Keys.OemQuestion:   // / key
                    SimulateOperatorClick("÷");
                    break;

                // Decimal point
                case Keys.OemPeriod:     // . key
                    SimulateDecimalClick();
                    break;

                // Equals
                case Keys.Enter:
                    SimulateEqualsClick();
                    break;

                // Clear functions
                case Keys.Back:          // Backspace - remove one digit
                    rmBtn_Click(null, null);
                    break;
                case Keys.Escape:        // Escape - clear all
                    EraseWholeNumBtn_Click(null, null);
                    break;

                // Toggle sound
                case Keys.S:
                    if (e.Control)  // Ctrl+S toggles sound
                    {
                        soundEnabled = !soundEnabled;
                        // Visual feedback
                        string soundStatus = soundEnabled ? "ON" : "OFF";
                        textExpression.Text = $"Sound: {soundStatus}";
                        if (soundEnabled) PlayButtonSound();
                    }
                    break;
            }
        }

        private void SimulateNumberClick(string number)
        {
            Button mockButton = new Button() { Text = number };
            Number_Click(mockButton, null);
        }

        private void SimulateOperatorClick(string operatorText)
        {
            Button mockButton = new Button() { Text = operatorText };
            Operator_Click(mockButton, null);
        }

        private void SimulateDecimalClick()
        {
            Decimal_Click(null, null);
        }

        private void SimulateEqualsClick()
        {
            btnEquals_Click(null, null);
        }


        private void InitOperations()
        {
            operations = new Dictionary<string, Func<Operation>>()
            {
                { "+", () => new Addition() },
                { "-", () => new Subtraction() },
                { "x", () => new Multiplication() },
                { "÷", () => new Division() },
                { "^", () => new Power() },
                { "√", () => new SquareRoot() },
            };
        }



        void eraseDisplayIfOpDone()
        {
            if (isOpDone)
            {
                firstNumber = secondNumber = 0;
                operation = "";
                textDisplay.Clear();
                textExpression.Clear();
                isOpDone = false;
            }
        }

        //If textDisplay.Text is null or empty, return false
        private bool TryParseDisplay(out double result)
        {
            result = 0;
            if(string.IsNullOrEmpty(textDisplay.Text))
            {
                return false;
            }
            return double.TryParse(textDisplay.Text, out result);
        }

        private double PerformCalculation(double first, double second, string op)
        {
            if(!operations.ContainsKey(op))
            {
                throw new InvalidOperationException("Invalid operation");
            }

            if(op == "÷" && second == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }

            Operation operation = operations[op]();
            return operation.Execute(first, second);
        }

        void pressedEqualsFunc()
        {
            textExpression.Clear();
            textDisplay.Text = "";
            firstNumber = 0;
            operation = "";
            expressionChain = ""; 
            justPressedEquals = false;
        }

        private void Number_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            PlayButtonSound();

            if (justPressedOperator)
            {
                textDisplay.Text = "";
                justPressedOperator = false;
                isOpDone = false;
            }
            else if(justPressedEquals)
            {
                pressedEqualsFunc();
            } else if (isOpDone)
            {
                eraseDisplayIfOpDone();
            }

            //if (justPressedEquals && string.IsNullOrEmpty(operation))
            //{
            //    pressedEqualsFunc();
            //}
            //else if (justPressedEquals)
            //{
            //    justPressedEquals = false;
            //    isOpDone = false;
            //}
            //else
            //{
            //    eraseDisplayIfOpDone();
            //}

            if (textDisplay.Text == "0" && button.Text == "0")
            {
                return;
            }

            if(textDisplay.Text == "0" && button.Text != "0")
            {
                textDisplay.Text = button.Text;
            }
            else
            {
                textDisplay.Text += button.Text;
            }
        }

        private void Decimal_Click(object sender, EventArgs e)
        {
            if (justPressedOperator)
            {
                textDisplay.Text = "0.";
                justPressedOperator = false;
                return;
            }

            if(justPressedEquals)
            {
                textExpression.Clear();
                textDisplay.Text = "0.";
                firstNumber = 0;
                operation = "";
                justPressedEquals = false;
                isOpDone = false;
                return;
            }

            if(isOpDone) isOpDone = false;

            if (isOpDone && (string.IsNullOrEmpty(textDisplay.Text) || textDisplay.Text == "0"))
            {
                eraseDisplayIfOpDone();
            }

            if (string.IsNullOrEmpty(textDisplay.Text) || textDisplay.Text == "0")
            {
                textDisplay.Text = "0.";
            }
            else if (!textDisplay.Text.Contains("."))
            {
                textDisplay.Text += ".";
            }
        }

        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            PlayButtonSound();
            if (!TryParseDisplay(out double currentNumber))
            {
                currentNumber = 0;
                textDisplay.Text = "0";
            }
            
            if (justPressedEquals)
            {
                firstNumber = currentNumber;
                operation = button.Text;
                expressionChain = firstNumber + " ";
                textExpression.Text = expressionChain + button.Text;
                justPressedEquals = false;
                justPressedOperator = true;
                return;
            }
            
            if (!string.IsNullOrEmpty(operation) && !justPressedOperator)
            {
                try
                {
                    double result = PerformCalculation(firstNumber, currentNumber, operation);

                    if (double.IsNaN(result) || double.IsInfinity(result))
                    {
                        textDisplay.Text = "Error";
                        textExpression.Text = "Error";
                        isOpDone = true;
                        return;
                    }

                    if (string.IsNullOrEmpty(expressionChain))
                        expressionChain = firstNumber + " " + operation + " " + currentNumber + " ";
                    else
                        expressionChain += operation + " " + currentNumber + " ";

                    firstNumber = result;
                    textDisplay.Text = result.ToString();

                    textExpression.Text = expressionChain + button.Text;
                }
                catch (Exception ex)
                {
                    textDisplay.Text = "Error";
                    textExpression.Text = "Error";
                    isOpDone = true;
                    return;
                }
            }
            else
            {
                firstNumber = currentNumber;
                expressionChain = firstNumber + " ";
                textExpression.Text = expressionChain + button.Text;
            }

            operation = button.Text;
            justPressedOperator = true;
        }

        private void btnEquals_Click(object sender, EventArgs e) { 
            PlayButtonSound();
            //secondNumber = if textDisplay.Text not empty then Convert.ToDouble(textDisplay.Text) else 0
            if(!TryParseDisplay(out double secondNumber)) //If it's true that it can parse, then assign the parsed value to secondNumber
            {
                secondNumber = 0;
            }

            if (justPressedEquals)
            {
                // Repeat the last operation
                if (!string.IsNullOrEmpty(operation))
                {
                    if (!TryParseDisplay(out double currentResult))
                        return;

                    try
                    {
                        double result = PerformCalculation(currentResult, this.secondNumber, operation);
                        textDisplay.Text = result.ToString();
                        textExpression.Text = currentResult + " " + operation + " " + this.secondNumber + " = " ;
                    }
                    catch (Exception ex)
                    {
                        textDisplay.Text = "Error";
                        textExpression.Text = "Error";
                    }
                }
                return;
            }

            if (operations.ContainsKey(operation))
            {
                if (operation == "÷" && secondNumber == 0)
                {
                    textDisplay.Text = "Cannot divide by 0";
                    textExpression.Text = firstNumber + " " + operation + " " + secondNumber + " = ";
                    isOpDone = true;
                    return;
                }
                try
                {
                    Operation op = operations[operation](); // Create instance of the Operation, operations(is the dictionary) at key operation (like +, -, etc)
                    double result = op.Execute(firstNumber, secondNumber);

                    if (double.IsNaN(result) || double.IsInfinity(result))
                    {
                        textDisplay.Text = "Error";
                        textExpression.Text = firstNumber + " " + operation + " " + secondNumber + " = ";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(expressionChain))
                        {
                            textExpression.Text = firstNumber + " " + operation + " " + secondNumber + " = ";
                            textDisplay.Text = result.ToString();
                        }
                        else
                        {
                            textDisplay.Text = result.ToString();
                            textExpression.Text = expressionChain + operation + " " + secondNumber + " = " ;
                        }
                    }
                    this.secondNumber = secondNumber;
                    justPressedEquals = true;
                    isOpDone = true;
                }
                catch(Exception ex)
                {
                    textDisplay.Text = "Error";
                    textExpression.Text = "Error";
                    isOpDone = true;
                }
            }
        }


        private void rmBtn_Click(object sender, EventArgs e)
        {
            justPressedOperator = false;
            justPressedEquals = false;

            if (textDisplay.Text.Length > 0)
            {
                textDisplay.Text = textDisplay.Text.Remove(textDisplay.Text.Length - 1);
                if(string.IsNullOrEmpty(textDisplay.Text))
                {
                    textDisplay.Text = "0";
                }
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            textDisplay.Clear();
            secondNumber = 0;
        }

        private void signBtn_Click(object sender, EventArgs e)
        {
            if(TryParseDisplay(out double currentNumber))
            {
                if(currentNumber == 0)
                {
                    return;
                }

                currentNumber = -currentNumber;
                textDisplay.Text = currentNumber.ToString();
                justPressedOperator = false;
                justPressedEquals = false;
            }
        }

        private void EraseWholeNumBtn_Click(object sender, EventArgs e)
        {
            textDisplay.Text = "0";
            textExpression.Clear();
            firstNumber = secondNumber = 0;
            operation = "";
            expressionChain = "";
            isOpDone = false;
            justPressedOperator = false;
            justPressedEquals = false;
        }

        private void Percent_Click(object sender, EventArgs e)
        {
            if(TryParseDisplay(out double num))
            {
                double result = num/ 100.0;
                textDisplay.Text = result.ToString();
                textExpression.Text = num + " % = ";
                isOpDone = true;
                justPressedEquals = false;
                justPressedOperator = false;
            }
            else
            {
                textDisplay.Text = "0";
            }
        }

        private void CalcForm_Load(object sender, EventArgs e)
        {

        }

        

        private void btnSqrt_Click(object sender, EventArgs e)
        {
            if (!TryParseDisplay(out double num))
            {
                textDisplay.Text = "Error";
                textExpression.Text = "Error";
                isOpDone = true;
                return;
            }

            if (num < 0)
            {
                textDisplay.Text = "Cannot calculate square root of negative number";
                textExpression.Text = "√" + num + " = Error";
                isOpDone = true;
                return;
            }

            try
            {
                Operation op = operations["√"]();
                double result = op.Execute(num, 0);
                textDisplay.Text = result.ToString();
                textExpression.Text = "√" + num + " = ";
                isOpDone = true;
                justPressedEquals = false;
                justPressedOperator = false;
            }
            catch (Exception ex)
            {
                textDisplay.Text = "Error";
                textExpression.Text = "Error";
                isOpDone = true;
            }
        }
    }
}
