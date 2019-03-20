namespace MEF.Launcher.Control
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    [TemplatePart(Name = PART_Main, Type = typeof(Border))]
    [TemplatePart(Name = PART_Close, Type = typeof(Button))]
    [TemplatePart(Name = PART_Min, Type = typeof (Button))]
    [TemplatePart(Name = PART_Max, Type = typeof (Button))]
    [TemplatePart(Name = PART_Close, Type = typeof (Button))]
    public class WindowHeader : ContentControl
    {
        #region Constants

        private const string PART_Min = "PART_Min";

        private const string PART_Max = "PART_Max";

        private const string PART_Close = "PART_Close";

        private const string PART_Title = "PART_Title";

        private const string PART_Main = "PART_Main";

        #endregion

        #region Variables

        /// <summary>
        /// The minimum button
        /// </summary>
        private Button minButton;

        /// <summary>
        /// The maximum button
        /// </summary>
        private Button maxButton;

        /// <summary>
        /// The close button
        /// </summary>
        private Button closeButton;

        /// <summary>
        /// The title text block
        /// </summary>
        private TextBlock titleTextBlock;

        /// <summary>
        /// The main border
        /// </summary>
        private Border mainBorder;

        #endregion

        #region Properties

        /// <summary>
        /// The root window property
        /// </summary>
        public static readonly DependencyProperty RootWindowProperty = DependencyProperty.Register("RootWindow", typeof(Window), typeof(WindowHeader));

        /// <summary>
        /// The window state property
        /// </summary>
        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register("WindowState", typeof(WindowState), typeof(WindowHeader));

        /// <summary>
        /// The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WindowHeader), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(TitleChangedCallback)));

        /// <summary>
        /// Gets or sets the root window.
        /// </summary>
        /// <value>
        /// The root window.
        /// </value>
        public Window RootWindow
        {
            get { return (Window) this.GetValue(RootWindowProperty); }
            set { this.SetValue(RootWindowProperty, value); }
        }

        /// <summary>
        /// Gets or sets the state of the window.
        /// </summary>
        /// <value>
        /// The state of the window.
        /// </value>
        public WindowState WindowState
        {
            get { return (WindowState)this.GetValue(WindowStateProperty); }
            set { this.SetValue(WindowStateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return (string)this.GetValue(RootWindowProperty); }
            set { this.SetValue(RootWindowProperty, value); }
        }

        #endregion

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.minButton = (Button) this.GetTemplateChild(PART_Min);
            this.maxButton = (Button) this.GetTemplateChild(PART_Max);
            this.closeButton = (Button) this.GetTemplateChild(PART_Close);
            this.titleTextBlock = (TextBlock) this.GetTemplateChild(PART_Title);
            this.mainBorder = (Border)this.GetTemplateChild(PART_Main);

            this.RegisterEvent();
        }

        /// <summary>
        /// Registers the event.
        /// </summary>
        private void RegisterEvent()
        {
            this.minButton.Click += (sender, args) =>
            {
                this.RootWindow.WindowState = WindowState.Minimized;
            };

            this.maxButton.Click += (sender, args) =>
            {
                this.AdjustWindow();
            };

            this.closeButton.Click += (sender, args) =>
            {
                this.RootWindow.Close();
            };

            this.mainBorder.MouseLeftButtonDown += (sender, args) =>
            {
                if (args.ClickCount == 2)
                {
                    this.AdjustWindow();
                }
                else
                {
                    this.RootWindow.DragMove();
                }
            };
        }

        /// <summary>
        /// Maximize or restore window
        /// </summary>
        private void AdjustWindow()
        {
            if (this.RootWindow.WindowState == WindowState.Normal)
            {
                this.RootWindow.WindowState = WindowState.Maximized;
            }
            else if (this.RootWindow.WindowState == WindowState.Maximized)
            {
                this.RootWindow.WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// Titles the changed callback.
        /// </summary>
        /// <param name="o">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void TitleChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var header = (WindowHeader) o;
            header.titleTextBlock.Text = e.NewValue.ToString();
        }
    }
}
