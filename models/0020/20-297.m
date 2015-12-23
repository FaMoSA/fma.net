A1 : C2 [A3] [A4] C5 [C6] C7 [A8] :: _A1 ;

A3 : C10 [C11] [C12] [C13] C14 :: _A3 ;

A4 : C15
	| C16
	| C17
	| C18
	| C19
	| C20 ;

A8 : C9 :: _A8 ;

%%

(not C6 iff not C15) and C10 implies A8 iff C15 ;
(not C11 or not C7) and not C11 iff not C14 ;

