A1 : C2 C3 [C4] C5 C6 C7 [C8] A9* A10 :: _A1 ;

A9 : C11
	| C12 ;

A10 : C13 C14 [C15] [C16] C17 C18 C19 [C20] :: _A10 ;

%%

(A9 iff not A10) and C16 ;
not (A1 or A10 or C11) implies C4 ;

