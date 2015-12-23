A1_ : A1+ :: _A1 ;

A1 : A2+ :: A2_
	| C3
	| C4 ;

A2 : C5
	| C6
	| C7
	| C8
	| C9
	| C10 ;

%%

not ((not A2 iff C4 iff not A1) implies A2) ;

