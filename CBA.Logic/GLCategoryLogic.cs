using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class GLCategoryLogic : BaseLogic<GLCategory, GLCategoryDAO>
    {
        //Get glCategory by name
        public GLCategory GetGLCategoryByName(string name)
        {
            return Dao.GetGLCategoryByName(name);
        }

        public bool CheckName(string text)
        {
            return Dao.CheckName(text);
        }

        public bool IsGLCategoryNameAvailable(string name)
        {
            if (Dao.GetGLCategoryByName(name) != null)
            {
                return false;
            }
            return true; 
        }
    }
}
