 .CODE

;-------------------------------------------------------------------------
;-------------------------------------------------------------------------

AsmMultiplication PROC result :dword;

    mov result, 0  
    mov R9, RDX ; row to multiply
l:   
        mov EAX, [R9]
        mul qword ptr [R8] ; multiplying row element by column element
        add result, EAX ; updating result
        add R9, 4 ; moving through rows array
        add R8, 4 ; moving through columns array
        dec RCX ; decrementing loop counter
        jnz l ; if loop counter == 0 stop
        mov EAX, result ; return result
        ret 

AsmMultiplication ENDP

end