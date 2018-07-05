/*
 * 名称：控件拖放类。
 * 
 * 作用：可以对控件进行拖放，只要是由Control类派生的类都可以。
 *       用法非常简单，ControlDrager cd = new ControlDrager(control);即可。
 * 
 * 时间：2011.6.9   初步建立这几个方法。     
 * 
 */
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindGoes.Forms
{
    /// <summary>
    /// 控件拖放类，只要是控件，就可以使用这个类进行拖放。
    /// </summary>
	public class ControlDrager : ObjectBase
    {
        private Control control;
        /// <summary>
        /// 所关联的控件。
        /// </summary>
        public Control Control
        {
            get { return control; }
            set { control = value; }
        }

        private bool allowDrag = true;
        /// <summary>
        /// 是否请允许拖放。
        /// </summary>
        public bool AllowDrag
        {
            get
            {
                return allowDrag;
            }
            set
            {
                allowDrag = value;
            }
        }
        
        /// <summary>
        /// 控件拖放类，直接把控件放进来即可。
        /// </summary>
        /// <param name="c"></param>
        public ControlDrager(Control c)
        {
            control = c;
            c.MouseDown += new MouseEventHandler(form_MouseDown);
            c.MouseMove += new MouseEventHandler(form_MouseMove);
            c.MouseUp += new MouseEventHandler(form_MouseUp); 
        }

        public void Depart()
        {
            try
            {
                control.MouseDown -= new MouseEventHandler(form_MouseDown);
                control.MouseMove -= new MouseEventHandler(form_MouseMove);
                control.MouseUp -= new MouseEventHandler(form_MouseUp); 
            }
            catch { }
        }

        #region 鼠标操作.

        bool isMouseDown = false; 
        Point lastPosition;
        Point mouseDownPosition;

        void form_MouseDown(object sender, MouseEventArgs e)
        {
            if (AllowDrag)
            {
                isMouseDown = true;
                lastPosition = control.Location;
                mouseDownPosition = Control.MousePosition;
            }
        }

        void form_MouseMove(object sender, MouseEventArgs e)
        { 
            if (isMouseDown)
            {
                control.Location = new Point(lastPosition.X + Control.MousePosition.X - mouseDownPosition.X,
                    lastPosition.Y + Control.MousePosition.Y - mouseDownPosition.Y);
            }
        }

        void form_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        #endregion

    }

}
