A1 : C2
	| C3
	| A4 ;

A4 : C40 [C41] :: A5
	| C6
	| C7
	| C8
	| A14 C15 C16 C17 A18 :: A9
	| C10
	| C12 [C13] :: A11 ;

A14 : A19 [C20] [C21] C22 [C23] :: _A14 ;

A19 : C25
	| C26
	| C27
	| C28
	| A29+ :: A29_ ;

A29 : C30
	| A31+ :: A31_
	| C32
	| C33
	| C34
	| A35
	| C36
	| C37
	| C38 ;

A31 : C39 ;

A35 : C48 [C49] [C50] :: A47 ;

A18 : A24+ :: _A18 ;

A24 : C42
	| C43
	| C44
	| C45
	| C46 ;

%%

not (C26 or A35 implies C46) iff not A24 iff not C13 ;
C34 or not C39 ;
not A19 or C42 implies A1 ;
(C46 iff A1) or A5 ;
((A14 implies not C49) implies not C23) implies not C48 iff C49 ;

