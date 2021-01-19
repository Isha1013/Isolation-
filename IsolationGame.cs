using System;
using static System.Console;
using static System.Math;

namespace Bme121
{
    static class Program
    {
        static string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
                
        static string playerA = "Player A";
        static string playerB = "Player B";
        
        static int numberOfRows = 6;
        static int numberOfCols = 8;
        
        static int platformARow;
        static int platformACol;
     
        static int platformBRow;
        static int platformBCol;
        
        static string move;
        
        static int pawnRowA;
        static int pawnColA;
        static int pawnRowB;
        static int pawnColB;
        static int removeRow;
        static int removeCol;
        
        static int nextPawnRow;
        static int nextPawnCol;
        static int nextRemoveRow;
        static int nextRemoveCol;
        
        static bool playerATurn = true;
        
        static bool isValid = false;
        
        static bool[ , ] board; 
        
        static void Main( )
        {
            Initialise();
            DrawGameBoard();
            for( int i = 1; i > 0; i++ ) //infinite loop
            {
                MakeMove();
                DrawGameBoard();
                playerATurn = !playerATurn;
                isValid = false;
            }
        }
        
        static void Initialise()
        {
            //Asking for player names  
            Write( "Please enter name for Player A: " );
            string responseA = ReadLine( );
            if( responseA.Length >= 1 ) playerA = responseA;
            WriteLine( playerA );
            
            Write( "Please enter name for Player B: " );
            string responseB = ReadLine( );
            if( responseB.Length >= 1 ) playerB = responseB;
            WriteLine( playerB );
            
            //Asking for number of rows and columns and checking if the input is valid 
            
            bool responseRowsValid = false;
            
            while( !responseRowsValid )
            {
                Write( "Please enter number of rows: " );
                int responseRows = int.Parse(ReadLine( ));
                
                responseRowsValid = true;
                
                if( responseRows < 4 || responseRows > 26 ) 
                {
                    Write( "Number of rows should not be lesser than 4 or greater than 26." );
                    WriteLine();
                    responseRowsValid = false;
                }
                else numberOfRows = responseRows;
             
            }
            
            bool responseColsValid = false;
            
            while( !responseColsValid )
            {
                Write( "Please enter number of columns: " );
                int responseCols = int.Parse(ReadLine( ));
                
                responseColsValid = true;
                
                if( responseCols < 4 || responseCols > 26 ) 
                {
                     Write( "Number of columns should not be lesser than 4 or greater than 26." );
                     WriteLine();
                     responseColsValid = false;
                }
                else numberOfCols = responseCols;
            }
            
            //Initialising board array
            board = new bool[ numberOfRows , numberOfCols ];
            
            for( int r = 0; r < board.GetLength(0); r++ )
                for( int c = 0; c < board.GetLength(1); c++ )
                    board[r,c] = true;
                
            //Setting default values for platforms
            platformARow = 2;
            platformACol = 0;
     
            platformBRow = 3;
            if( board.GetLength(1) < 8 ) platformBCol = (int) Math.Ceiling( board.GetLength(1)/ 2.0 );
            else platformBCol = 7;
            
            //Asking for starting platforms
            Write( "Please enter starting platform for {0} in the format row,column: ", playerA );
            string platformA = ReadLine();
            
            //Extracting row and column separately for player A
            string aRow = platformA.Substring(0,1);
            int tempARow = Array.IndexOf( letters, aRow );
            if( tempARow >= 0 && tempARow < board.GetLength(0) ) platformARow = tempARow;
            
            string aCol = platformA.Substring(2,1);
            int tempACol = Array.IndexOf( letters, aCol );
            if( tempACol >= 0 && tempACol < board.GetLength(1) ) platformACol = tempACol;
            
            Write( "Please enter starting platform for {0} in the format row,column: ", playerB );
            string platformB = ReadLine();
            
            //Extracting row and coloumn separately for player B
            string bRow = platformB.Substring(0,1);
            int tempBRow = Array.IndexOf( letters, bRow );
            if( tempBRow >= 0 && tempBRow < board.GetLength(0) ) platformBRow = tempBRow;
            
            string bCol = platformB.Substring(2,1);
            int tempBCol = Array.IndexOf( letters, bCol );
            if( tempBCol >= 0 && tempBCol < board.GetLength(1)) platformBCol = tempBCol;

            pawnRowA = platformARow;
            pawnColA = platformACol;
            pawnRowB = platformBRow;
            pawnColB = platformBCol;
        } 
        
        static void DrawGameBoard( )
        {
            
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string sp = " ";      // space
            const string pa = "A";      // pawn A
            const string pb = "B";      // pawn B
            const string bb = "\u25a0"; // block
            const string fb = "\u2588"; // left half block
            const string lh = "\u258c"; // left half block
            const string rh = "\u2590"; // right half block
            

            for( int c = 0; c < board.GetLength( 1 ); c ++ )
            {
                if( c == 0 ) Write("     {0} ", letters[c] );
                else Write( "  {0} ", letters[c] ); 
            }
            WriteLine();
            
            // Drawing top boundary
            Write( "   " );
            for( int c = 0; c < board.GetLength( 1 ); c ++ )
            {
                if( c == 0 ) Write( tl );
                Write( "{0}{0}{0}", h );
                if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", tr ); 
                else Write( "{0}", hb );
            }
            WriteLine( );
            
            // Drawing board rows
            for( int r = 0; r < board.GetLength( 0 ); r ++ )
            {
                Write( " {0} ", letters[ r ] );
                
                // Drawing row contents
                for( int c = 0; c < board.GetLength( 1 ); c ++ )
                {
                    if( c == 0 ) Write( v );
                    
                    if( isValid &&  r == pawnRowA && c == pawnColA ) Write( "{0}{1}{2}{3}",sp,pa,sp,v );
                    else if( isValid &&  r == pawnRowB && c == pawnColB ) Write( "{0}{1}{2}{3}",sp,pb,sp,v );
                    
                    else if( r == platformARow && c == platformACol ) Write( "{0}{1}{2}{3}",sp,bb,sp,v );
                    else if( r == platformBRow && c == platformBCol ) Write( "{0}{1}{2}{3}",sp,bb,sp,v ); 

                    else if ( board[ r, c ] ) Write( "{0}{1}{2}{3}",rh,fb,lh,v );
                    
                    else Write( "{0}{1}", "   ", v );
                }
                WriteLine( );
                
                // Drawing boundary after the row
                if( r != board.GetLength( 0 ) - 1 )
                { 
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( vr );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", vl ); 
                        else                                Write( "{0}", hv );
                    }
                    WriteLine( );
                }
                else
                {
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( bl );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", br ); 
                        else                                Write( "{0}", ha );
                    }
                    WriteLine( );
                }
            }
        }
        
        static void MakeMove( )
        {
            while( ! isValid )
            {
                //Prompting player for move
                if( playerATurn == true )
                {
                    Write( "{0} make your move: ", playerA );
                    move = ReadLine();
                    isValid = true;
                }
                else
                {
                    Write( "{0} make your move: ", playerB );
                    move = ReadLine();
                    isValid = true;
                }
                
                //Checking for number of characters in move
                if( move.Length != 4 ) 
                {
                    isValid = false;
                    Write( "Your move should have 4 characters." );
                    WriteLine();
                }
                 
                //Extracting individual row and column elements from move.
                string pawnR = move.Substring(0,1);
                string pawnC = move.Substring(1,1);
                string removeR = move.Substring(2,1);
                string removeC = move.Substring(3,1);
                
                nextPawnRow = Array.IndexOf( letters, pawnR );
                nextPawnCol = Array.IndexOf( letters, pawnC );
                nextRemoveRow = Array.IndexOf( letters, removeR );
                nextRemoveCol = Array.IndexOf( letters, removeC );
                
                //Checking if move is within boundaries of board
                if( nextPawnRow < 0 || nextPawnRow > board.GetLength(0) 
                    || nextPawnCol < 0 || nextPawnCol > board.GetLength(1) 
                    || nextRemoveRow < 0 || nextRemoveRow > board.GetLength(0) 
                    || nextRemoveCol < 0 || nextRemoveCol > board.GetLength(1) ) 
                {
                    isValid = false;
                    Write("Your coordinates should be within the bounds of the board.");
                    WriteLine();
                }
                
                //Checking if pawn can move
                if  ( board[nextPawnRow, nextPawnCol] == false 
                    || nextPawnRow == pawnRowA && nextPawnCol == pawnColA
                    || nextPawnRow == pawnRowB && nextPawnCol == pawnColB
                    || playerATurn && (int) Math.Abs( pawnRowA - nextPawnRow ) > 1
                    || playerATurn && (int) Math.Abs( pawnColA - nextPawnCol ) > 1
                    || ! playerATurn && (int) Math.Abs( pawnRowB - nextPawnRow ) > 1
                    || ! playerATurn && (int) Math.Abs( pawnColB - nextPawnCol ) > 1 )
                {
                    isValid = false;
                    Write("Your move is not valid.");
                    WriteLine();
                }
                
                //Checking if tile can be removed
                if  ( board[nextRemoveRow, nextRemoveCol] == false 
                    || nextRemoveRow == platformARow && nextRemoveCol == platformACol 
                    || nextRemoveRow == platformBRow && nextRemoveCol == platformBCol 
                    || ! playerATurn && nextRemoveRow == pawnRowA && nextRemoveCol == pawnColA
                    || playerATurn && nextRemoveRow == pawnRowB && nextRemoveCol == pawnColB
                    || nextRemoveRow == nextPawnRow && nextRemoveCol == nextPawnCol)
                {
                    isValid = false;
                    Write( "Your remove is not valid." );
                    WriteLine();
                }
            }
            
            //Executing move if input is valid
            if( isValid == true )
            {
                if( playerATurn )
                {
                    pawnRowA = nextPawnRow;
                    pawnColA = nextPawnCol;
                }
                else
                {
                    pawnRowB = nextPawnRow;
                    pawnColB = nextPawnCol;
                }
                
                removeRow = nextRemoveRow;
                removeCol = nextRemoveCol;
               
                board[removeRow, removeCol] = false; 
            }
        }
    }
}
