A1 : A2+ C3 C4 C5 C6 [C7] C8 :: _A1 ;

A2 : C9
	| C10 ;

%%

(C3 and not C6 implies C10) implies A2 ;

