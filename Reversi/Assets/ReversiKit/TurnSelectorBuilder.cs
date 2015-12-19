using System;

namespace ReversiKit
{
    public class TurnSelectorBuilder
    {
        private TurnSelectorBuilder()
        {
        }

        public static ITurnSelector CreateCornerAndGreedyTurnSelector()
        {
            var cornerSelector = new CornerTurnSelector();
            var borderSelector = new BorderTurnSelector();
            var greedySelector = new GreedyTurnSelector();

            ITurnSelector[] selectors = new ITurnSelector[3] 
            { 
                cornerSelector,  
                borderSelector,
                greedySelector
            };


            CompositeTurnSelector result = new CompositeTurnSelector(selectors);
            return result;
        }

    }
}

