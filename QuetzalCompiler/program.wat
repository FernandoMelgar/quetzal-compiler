(module
  (import "quetzal" "printi" (func $printi (param i32) (result i32)))
  (import "quetzal" "printc" (func $printc (param i32) (result i32)))
  (import "quetzal" "println" (func $println (result i32)))
  (import "quetzal" "readi" (func $readi (result i32)))
  (import "quetzal" "reads" (func $reads (result i32)))
  (import "quetzal" "new" (func $new (param i32) (result i32)))
  (import "quetzal" "size" (func $size (param i32) (result i32)))
  (import "quetzal" "add" (func $add (param i32 i32) (result i32)))
  (import "quetzal" "get" (func $get (param i32 i32) (result i32)))
  (import "quetzal" "set" (func $set (param i32 i32 i32) (result i32)))
  (global $global1 (mut i32) (i32.const 0))
  (global $global2 (mut i32) (i32.const 0))
  (func $sqrt)
    (param $z i32)
    (param $zz i32)
    (result i32)
    (local $z i32)
    (local $zz i32)
    (local $r i32)
    i32.const 0
  )
  (func
    (export "main")
    (result i32)
    (local $x i32)
    i32.const 4
    local.set $x
    local.get $x
    i32.const 4
    i32.mul
    local.set $x
    call $prints
    drop
    i32.const 0
  )
)
