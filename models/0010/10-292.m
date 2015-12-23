A1 : A2 [C3] A4* [C5] [C6] :: _A1 ;

A2 : C7
	| C8 ;

A4 : C9
	| C10 ;

%%

not (C9 and not A2 implies C3 iff not A4) ;

