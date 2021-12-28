.CODE

;-------------------------------------------------------------------------
;-------------------------------------------------------------------------

AsmMultiplication PROC outerLoopCount: dword, innerLoopCount: dword, startColAddress : qword, startRowAddress : qword, count : qword, columns : dword     

													; resultRow in RCX
													; rowToMultiply in RDX
													; colToMultiply in R8
													; argsPtr in R9 - [rows, columns]


mov EAX, [R9]
mov outerLoopCount, EAX
mov columns, EAX

mov EAX, [R9 + 4]
mov innerLoopCount, EAX

mov count, 0

mov R10, RDX
mov R9, RCX

mov startColAddress, R8
mov startRowAddress, R10

loop1:
mov R10, startRowAddress								; | row = startRowAddress

xor RAX, RAX											; |
mov [R9], EAX											; | (*resultRow) = 0 RAX

mov ECX, innerLoopCount									; |
pxor xmm2, xmm2											; | preparing for multiplying in loop2
pxor xmm3, xmm3
inc count
	
			loop2:
			movdqu xmm0, [R10]							; | move actual row element to vector bylo qword ptr
			movdqu xmm1, [R8]							; | move actual column element to vector bylo qword ptr
			pmulld xmm0, xmm1							; | multiply vectors
			;pxor xmm3, xmm3							: |
			;haddps xmm3, xmm0							; | 
			;haddps xmm3, xmm3							; | 
			;paddq xmm2, xmm3							; | add result to third vector
			paddq xmm3, xmm0

			add R10, 16									; | row++ 4
			add R8, 16									; | columns ++ 4


			dec ECX										; | decrementing innerLoopCounter...
			dec ECX										; |	
			dec ECX										; |	
			dec ECX										; |	
			jnz loop2									; | if innerLoopCounter == 0 break
haddps xmm3, xmm3										; | usumac jakby nie dzialalo
haddps xmm2, xmm3										; | usunac tez
pshufd xmm2, xmm2, 2
movq RAX, xmm2											; |
mov [R9], EAX											; | resultRow = rows * columns
add R9, 4												; | resultRows++ 


mov EDX, outerLoopCount									; |
dec EDX													; |
mov outerLoopCount, EDX									; |
jnz loop1												; | if outerLoopCounter == 0 break


ret
AsmMultiplication ENDP


end