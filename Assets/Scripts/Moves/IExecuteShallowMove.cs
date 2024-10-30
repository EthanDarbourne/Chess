using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Moves
{
    public interface IExecuteShallowMove
    {
        public void ExecuteShallowMove( ShallowBoard board );
    }
}
