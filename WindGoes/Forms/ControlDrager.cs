/*
 * ���ƣ��ؼ��Ϸ��ࡣ
 * 
 * ���ã����ԶԿؼ������Ϸţ�ֻҪ����Control���������඼���ԡ�
 *       �÷��ǳ��򵥣�ControlDrager cd = new ControlDrager(control);���ɡ�
 * 
 * ʱ�䣺2011.6.9   ���������⼸��������     
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
    /// �ؼ��Ϸ��ֻ࣬Ҫ�ǿؼ����Ϳ���ʹ�����������Ϸš�
    /// </summary>
	public class ControlDrager : ObjectBase
    {
        private Control control;
        /// <summary>
        /// �������Ŀؼ���
        /// </summary>
        public Control Control
        {
            get { return control; }
            set { control = value; }
        }

        private bool allowDrag = true;
        /// <summary>
        /// �Ƿ��������Ϸš�
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
        /// �ؼ��Ϸ��ֱ࣬�Ӱѿؼ��Ž������ɡ�
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

        #region ������.

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
