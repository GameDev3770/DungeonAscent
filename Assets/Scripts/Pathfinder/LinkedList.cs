using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace LL {

    public class Node<T> {
        Node<T> next;
        Node<T> prev;
        T value;

        public Node(T value, Node<T> next = null, Node<T> prev = null) {
            this.value = value;
            this.next = next;
            this.prev = prev;
        }

        public void AddAfter(Node<T> node) {
            Node<T> afterNode = this.next;
            this.next = node;
            node.next = afterNode;
            node.prev = this;
            if (afterNode != null) afterNode.prev = node;

        }
        public void AddBefore(Node<T> node) {
            Node<T> beforeNode = this.prev;
            this.prev = node;
            node.prev = beforeNode;
            node.next = this;
            if (beforeNode != null) beforeNode.next = node;
        }

        public T Seperate() {
            Node<T> beforeNode = this.prev;
            Node<T> afterNode = this.next;

            if (beforeNode != null) beforeNode.next = afterNode;
            if (afterNode != null) afterNode.prev = beforeNode;

            this.next = null;
            this.prev = null;
            return this.value;
        }

        public T Value {
            get { return this.value; }
        }
        public Node<T> Next {
            get { return this.next; }
        }
        public Node<T> Prev {
            get { return this.prev; }
        }
    }

    public class LinkedList<T> {
        public Node<T> first;
        public int length = 0;

        Func<T, T, bool> SortFunction;

        public LinkedList(Func<T, T, bool> sortFunction = null) {
            this.SortFunction = sortFunction == null ? DefaultSortFunction : sortFunction;
        }
        public LinkedList(T first, Func<T, T, bool> sortFunction = null) : this(sortFunction) {
            this.first = new Node<T>(first);
            if (this.first != null) length++;
        }

        public void Add(T item) {
            Node<T> node = new Node<T>(item);

            if (this.first == null) {
                this.first = node;
                length++;
                return;
            } Node<T> ptr = this.first;

            while (ptr.Next != null) {
                if (SortFunction(ptr.Value, node.Value)) {
                    ptr.AddAfter(node);
                    break;
                } ptr = ptr.Next;
            } 
            if (ptr.Next == null) {
                if (SortFunction(ptr.Value, node.Value)) ptr.AddBefore(node);
                else ptr.AddAfter(node);
            }
            if (this.first.Prev != null) this.first = this.first.Prev;

            length++;
        }

        public void Update(T node) {
            Node<T> ptr = this.first;
            while (ptr != null) {
                if (ptr.Value.Equals(node)) {
                    Update(ptr);
                    break;
                }
                ptr = ptr.Next;
            }
        }

        public void Update(Node<T> node) {
            Node<T> ptr = node.Next;
            node.Seperate();
            //length--;
            while (ptr != null) {
                if (SortFunction(node.Value, ptr.Value)) {
                    ptr.AddAfter(node);
                    break;
                }
                ptr = ptr.Prev;
            }
            if (ptr == null) {
                this.first.AddBefore(node);
                this.first = node;
            }
            //length++;
        }

        public T Push() {
            try {
                Node<T> temp = this.first;
                Node<T> nextTemp = temp.Next;
                this.first.Seperate();
                this.first = nextTemp;
                length--;
                return temp.Value;
            }
            catch (NullReferenceException e) {
                throw e;
            }
            
        }

        public bool Contains(T value) {
            for (Node<T> ptr = this.first; ptr != null; ptr = ptr.Next) {
                if (ptr.Value.Equals(value)) {
                    return true;
                }
            }
            return false;
        }










        bool DefaultSortFunction(T val1, T val2) {
            return true;
        }
    }
}


