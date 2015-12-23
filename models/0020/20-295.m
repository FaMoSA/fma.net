A1 : A2+ C3 C4 A5 C6 [C7] C8 :: _A1 ;

A2 : C9
	| C10
	| C11
	| C12
	| C13
	| C14 ;

A5 : C15 C16 C17 C18 [C19] [C20] :: _A5 ;

%%

not (C7 and not C14) or C14 ;
(not C13 or not C6 iff not A2) and not C19 iff C3 ;

