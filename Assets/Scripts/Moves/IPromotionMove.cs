using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Moves
{
    public interface IPromotionMove
    {
        public PieceType PromoteTo { get; }
    }
}
