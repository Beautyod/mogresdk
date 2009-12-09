// http://www.ogre3d.org/wiki/index.php/Mogre_Basic_Tutorial_6

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mogre;

namespace Tutorial06
{
   static class Program
   {
      [STAThread]
      
      static void Main()
      {
         OgreForm form = new OgreForm();
         form.Init();
         form.Go();
      }
   }
}
