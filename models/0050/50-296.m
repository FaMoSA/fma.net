A1 : A2 C3 A4* [C5] C6 A7* C8 [C9] C10 :: _A1 ;

A2 : A11+ C12 :: _A2 ;

A11 : C47
	| C48
	| C49
	| C50 ;

A4 : C13
	| C14
	| C15
	| C16
	| C17
	| A35+ C36 [C37] :: A18
	| C19
	| C20
	| A21+ :: A21_
	| C22 ;

A35 : C38
	| C39
	| C40
	| C41
	| C42
	| C43
	| C44
	| C45
	| C46 ;

A21 : C26
	| C27
	| C32 C33 [C34] :: A28
	| C29
	| C30
	| C31 ;

A7 : C23
	| C24
	| C25 ;

%%

(not C16 or not A7 implies not C40) or C27 ;
not (not ((not C16 or not C25) and C12) implies not C16) ;
(((not C24 implies not C43) implies not C13) or C36) and C10 ;
(not C14 implies C17 iff A18) implies C31 ;
(not (not C23 implies C26) iff A2) and C31 ;

